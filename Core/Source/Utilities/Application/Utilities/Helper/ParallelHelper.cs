using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.Utilities.Helper
{
    /// <summary>
    /// Ayudas para los procesos en paralelo
    /// </summary>
    public static class ParallelHelper
    {
        /// <summary>
        /// Ejecuta un ForEach en paralelo, ejecutandolo por paquetes
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">Listado a ejecutar en paralelo</param>
        /// <param name="bodyToExecute">Cuerpo del proceso a ejecutar en paralelo</param>
        public static void ForEach<TSource>(List<TSource> list, Action<TSource> bodyToExecute)
        {
            ForEach(list.ToList(), bodyToExecute, "MaxThreadMassive");
        }


        /// <summary>
        /// Ejecuta un ForEach en paralelo, ejecutandolo por paquetes
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">Listado a ejecutar en paralelo</param>
        /// <param name="bodyToExecute">Cuerpo del proceso a ejecutar en paralelo con paralalel loop state</param>
        public static void ForEach<TSource>(List<TSource> list, Action<TSource, ParallelLoopState> bodyToExecute)
        {
            ForEach(list.ToList(), bodyToExecute, "MaxThreadMassive");
        }


        /// <summary>
        /// Ejecuta un ForEach en paralelo, ejecutandolo por paquetes
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">Listado a ejecutar en paralelo</param>
        /// <param name="bodyToExecute">Cuerpo del proceso a ejecutar en paralelo</param>
        /// <param name="tagConfigPackage">Tag de configuracion para el maximo de paquetes</param>
        public static void ForEach<TSource>(List<TSource> list, Action<TSource> bodyToExecute, string tagConfigPackage)
        {
            List<int> packages = DataFacadeManager.GetPackageProcesses(list.Count(), tagConfigPackage);
            foreach (int package in packages)
            {
                List<TSource> packageList = list.Take(package).ToList();
                list.RemoveRange(0, package);
                TP.Parallel.ForEach(packageList, bodyToExecute);
            }
        }

        /// <summary>
        /// Ejecuta un ForEach en paralelo, ejecutandolo por paquetes
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list">Listado a ejecutar en paralelo</param>
        /// <param name="bodyToExecute">Cuerpo del proceso a ejecutar en paralelo con parallel loop state</param>
        /// <param name="tagConfigPackage">Tag de configuracion para el maximo de paquetes</param>
        public static void ForEach<TSource>(List<TSource> list, Action<TSource, ParallelLoopState> bodyToExecute, string tagConfigPackage)
        {
            List<int> packages = DataFacadeManager.GetPackageProcesses(list.Count(), tagConfigPackage);
            foreach (int package in packages)
            {
                List<TSource> packageList = list.Take(package).ToList();
                list.RemoveRange(0, package);
                Parallel.ForEach(packageList, bodyToExecute);
            }
        }

        /// <summary>
        /// Obtiene registros de un list aplicando distinct a determinada propiedad
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">list fuente</param>
        /// <param name="keySelector">propiedad</param>
        /// <author>Germán F. Grimaldi</author> <date>31/10/2018</date> <req>296</req>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (!seenKeys.Contains(keySelector(element)))
                {
                    seenKeys.Add(keySelector(element));
                    yield return element;
                }
            }
        }
        #region paralel extension
        /// <summary>
        /// Ejecutar ForEach en Paralelo
        /// </summary>
        /// <typeparam name="TSource">El tipo de Origen.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="body">The body.</param>
        /// <param name="parallelOptions">The parallel options.</param>
        public static void ForEachParallel<TSource>(this IEnumerable<TSource> source, Action<TSource> body, ParallelOptions parallelOptions = null, string key = "DebugParallel")
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            BusinessContext context = null;
            if (BusinessContext.Current != null)
            {
                context = BusinessContext.Current;
            }
            if (parallelOptions == null)
            {
                parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
            }
            // Parallel.ForEach(source, parallelOptions, body);
            Parallel.ForEach(source, parallelOptions, x =>
            {
                try
                {
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    body(x);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            });
        }

        /// <summary>
        /// Proceso Asincrono
        /// </summary>
        /// <param name="theAction">Accion o Delegado a Ejecutar.</param>
        /// <returns></returns>
        public static Task TaskContext(this Action theAction)
        {
            var context = BusinessContext.Current;
            return Task.Run(() =>
            {
                try
                {
                    if (BusinessContext.Current == null)
                    {
                        BusinessContext.Current = context;
                    }
                    theAction();
                }
                finally
                {
                    BusinessContext.Current = null;
                    DataFacadeManager.Dispose();
                }
            });
        }
        #endregion


        /// <summary>
        /// Depuracion Paralelo, Toma la cantidad de Hilos que este en archivo de Configuracion si es debug, Nomar toma MaxThreadCollective
        /// </summary>
        /// <param name="key">llave asignada en el Config.</param>
        /// <returns>
        /// ParallelOptions
        /// </returns>
        public static ParallelOptions DebugParallelFor(string key = "DebugParallel")
        {
            var parallelOptions = new ParallelOptions();
#if DEBUG
            parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = KeySettings.GetSetting(key) };
#else
            parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = KeySettings.GetSetting(key, Environment.ProcessorCount) };
#endif
            return parallelOptions;
        }



    }
}
