using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF.Engine.Configuration;
using Sistran.Core.Framework.DAF.Mapping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Utilities.Cache
{
    /// <summary>
    /// Cache Diccionarios
    /// </summary>
    public class DictionaryCache
    {
        static ConcurrentBag<string> errors = new ConcurrentBag<string>();
        /// <summary>
        /// Carga cache diccionarios
        /// </summary>
        public static void LoadCache()
        {
            try
            {

                errors = new ConcurrentBag<string>();
                PathCollection paths = new PathCollection();
                var ViewsDirectories = new List<string>();
                ViewsDirectories.AddRange(DafSection.GetSection().ViewsDirectories.GetPaths());
                paths.AddRange(ViewsDirectories);
                IList<string> Path = new List<string> { KeySettings.EntityPathBase };
                DirectoriesClassMapProvider provider = new DirectoriesClassMapProvider(Path);
                DirectoriesViewMapProvider DirectoriesViewMapProvider = new DirectoriesViewMapProvider(paths);
                Task.Run(() => DirectoriesViewMapProvider.LoadClassViewMaps());
                Task.Run(() => provider.LoadClassMaps()).ContinueWith(z =>
                {
                    if (KeySettings.InitializeCache)
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        EventLog.WriteEntry("ApplicationServiceCache", $"Precarga Iniciada -  {DateTime.Now}");
                        Task entity = Task.Run(() => LoadEntities(Path)).ContinueWith
                        (m =>
                        {
                            try
                            {
                                EventLog.WriteEntry("ApplicationServiceCache", $"Reglas : {DateTime.Now} - Tiempo transcurrido: { stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\.fff")}");
                                Task tr = Task.Run(() => RuleCache.LoadRuleCache());
                                tr.Wait();
                                stopwatch.Stop();
                                EventLog.WriteEntry("ApplicationServiceCache", $"Precarga finalizada : {DateTime.Now} - Tiempo transcurrido: { stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\.fff")}");
                            }
                            catch (Exception ex)
                            {

                                errors.Add($"{ex.Message}");
                            }
                        }
                        );
                        Task entityCompany = Task.Run(() => LoadEntitiesCompany(Path));
                        try
                        {
                            Task.WaitAll(entity, entityCompany);
                        }
                        catch (AggregateException ae)
                        {

                            EventLog.WriteEntry("ApplicationError", ae.GetBaseException().Message);
                        }


                    }
                });
            }
            catch (Exception ex)
            {

                errors.Add($"{ex.Message}");
                EventLog.WriteEntry("ApplicationError", "Company:" + ex.GetBaseException().Message);
            }
            finally
            {
                errors = new ConcurrentBag<string>();
            }

        }
        public static void LoadEntities(IList<string> PathDir)
        {
            try
            {

                IEnumerable<string> files = null;
                if (KeySettings.ExcludeFolders?.Count > 0)
                {
                    files = Directory.EnumerateFiles(KeySettings.EntityPath, DllValue.Dll, SearchOption.AllDirectories).Where(s => !KeySettings.ExcludeFolders.Any(d => Path.GetDirectoryName(s).Contains(d, StringComparison.OrdinalIgnoreCase)));
                }
                else
                {
                    files = Directory.EnumerateFiles(KeySettings.EntityPath, DllValue.Dll, SearchOption.AllDirectories);
                }
                int countCache = 0;
                if (files?.Count() > 0)
                {
                    int maxThread = files.Count() >= Environment.ProcessorCount ? Environment.ProcessorCount - 2 : files.Count();
                    maxThread = maxThread > 9 ? 7 : maxThread;
                    DirectoriesClassMapProvider provider = new DirectoriesClassMapProvider(PathDir);
                    Parallel.For(0, files.Count(), new ParallelOptions { MaxDegreeOfParallelism = maxThread }, a =>
                    {
                        try
                        {
                            AssemblyName assemblyName = AssemblyName.GetAssemblyName(files.ElementAt(a));
                            var assembly = Assembly.Load(assemblyName);
                            if (assembly != null && assembly.GetTypes() != null && assembly.GetTypes().Length > 0)
                            {
                                var assemblyData = assembly.GetTypes().Where(y => y.CustomAttributes.Where(n => n.AttributeType.Name == "PersistentClassAttribute").FirstOrDefault()?.AttributeType?.Name == "PersistentClassAttribute");
                                Parallel.For(0, assemblyData.Count(), new ParallelOptions { MaxDegreeOfParallelism = maxThread * 2 }, q =>
                                {
                                    try
                                    {

                                        if (assemblyData.ElementAt(q) != null && !string.IsNullOrEmpty(assemblyData.ElementAt(q).FullName))
                                        {
                                            provider.GetClassMap(assemblyData.ElementAt(q), false);
                                            Interlocked.Increment(ref countCache);
                                        }   

                                    }
                                    catch (Exception ex)
                                    {

                                        errors.Add($"{assembly.FullName} - {assemblyData.ElementAt(q).FullName} : {ex.Message}");
                                    }

                                });
                            }
                        }
                        catch (Exception ex)
                        {

                            errors.Add($"{ex.Message}");
                        }
                        finally
                        {
                            DataFacade.DataFacadeManager.Dispose();
                        }
                    });
                }
                EventLog.WriteEntry("ApplicationServiceCache", $"Entidades : {DateTime.Now} - Cantidad: {countCache}");
                if (errors.Count > 0)
                {
                    EventLog.WriteEntry("ApplicationError", string.Join("-", errors));
                }
            }
            catch (Exception ex)
            {

                errors.Add($"{ex.Message}");
                EventLog.WriteEntry("ApplicationError", ex.GetBaseException().Message);

            }

        }

        /// <summary>
        /// Loads the entities company.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void LoadEntitiesCompany(IList<string> PathDir)
        {
            try
            {
                int countCache = 0;
                IEnumerable<string> filesCompany = null;
                if (KeySettings.ExcludeFolders?.Count > 0)
                {
                    filesCompany = Directory.EnumerateFiles(KeySettings.EntityPathCompany, DllValue.Dll, SearchOption.AllDirectories).Where(s => !KeySettings.ExcludeFolders.Any(d => Path.GetDirectoryName(s).Contains(d, StringComparison.OrdinalIgnoreCase)));
                }
                else
                {
                    filesCompany = Directory.EnumerateFiles(KeySettings.EntityPathCompany, DllValue.Dll, SearchOption.AllDirectories);
                }
                if (filesCompany?.Count() > 0)
                {
                    int maxThread = filesCompany.Count() >= Environment.ProcessorCount ? Environment.ProcessorCount - 4 : filesCompany.Count();
                    maxThread = maxThread > 9 ? 7 : maxThread;
                    Parallel.For(0, filesCompany.Count(), new ParallelOptions { MaxDegreeOfParallelism = maxThread }, a =>
                      {
                          try
                          {
                              AssemblyName assemblyName = AssemblyName.GetAssemblyName(filesCompany.ElementAt(a)); var assembly = Assembly.Load(assemblyName);

                              if (assembly != null && assembly.GetTypes() != null && assembly.GetTypes().Length > 0)
                              {
                                  var assemblyData = assembly.GetTypes().Where(y => y.CustomAttributes.Where(n => n.AttributeType.Name == "PersistentClassAttribute").FirstOrDefault()?.AttributeType?.Name == "PersistentClassAttribute");
                                  DirectoriesClassMapProvider provider = new DirectoriesClassMapProvider(PathDir);
                                  Parallel.ForEach(assemblyData, new ParallelOptions { MaxDegreeOfParallelism = maxThread * 2 }, q =>
                                    {
                                        try
                                        {

                                            if (q != null && !string.IsNullOrEmpty(q.FullName))
                                            {                                               
                                                provider.GetClassMap(q, false);
                                                Interlocked.Increment(ref countCache);
                                            }

                                        }
                                        catch (Exception ex)
                                        {

                                            errors.Add($"{assembly.FullName} - {q.FullName} : {ex.Message}");
                                        }

                                    });
                              }
                          }
                          catch (Exception ex)
                          {

                              errors.Add($"{ex.Message}");
                          }
                      });                 
                }
                EventLog.WriteEntry("ApplicationServiceCache", $"Entidades Company : {DateTime.Now} - Cantidad: {countCache}");
                if (errors.Count > 0)
                {
                    EventLog.WriteEntry("ApplicationError", string.Join("-", errors));
                }
            }
            catch (Exception ex)
            {

                errors.Add($"{ex.Message}");
                EventLog.WriteEntry("ApplicationError", ex.GetBaseException().Message);
            }

        }

    }
    static class ExtensionContains
    {

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }

}
