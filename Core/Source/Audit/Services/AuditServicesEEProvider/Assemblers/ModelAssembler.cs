namespace Sistran.Core.Application.AuditServices.EEProvider.Assemblers
{
    using AutoMapper;
    using Newtonsoft.Json;
    using Sistran.Core.Application.AuditServices.EEProvider.Helpers;
    using Sistran.Core.Application.AuditServices.Enums;
    using Sistran.Core.Application.AuditServices.Models;
    using Sistran.Core.Application.RulesScriptsServices.EEProvider.Helper;
    using Sistran.Core.Application.UniqueUser.Entities;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using TM=System.Threading.Tasks;
    using AUDEN = Audit.Entities;
    using AUDModel = AuditServices.Models;
    using COMMEN = Common.Entities;
    using COMMENUM = CommonService.Enums;
    using COMMModel = CommonService.Models;
    using System.Threading.Tasks;

    public class ModelAssembler
    {
        #region Audit

        /// <summary>
        /// Creates the tax.
        /// </summary>
        /// <param name="audit">The audit.</param>
        /// <returns></returns>
        public static AUDModel.Audit CreateAudit(AUDEN.LogAudit audit)
        {
            var changes = JsonConvert.DeserializeObject<List<AuditChange>>(audit.Data);
            if (audit.IsSerialize)
            {
                changes = null;
                changes.Add(ReadObject(changes.Where(x => x.IsSerialize = true).FirstOrDefault()));
            }
            return new AUDModel.Audit
            {
                IsSerialize = audit.IsSerialize,
                ActionType = (AudictType?)audit.TypeCode,
                RegisterDate = audit.RegisterDate,
                ObjectName = audit.ObjectDescription,
                User = new User { Id = (int)audit.UserId },
                Changes = changes
            };
        }


        /// <summary>
        /// Creates the tax.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<AUDModel.Audit> CreateAudits(BusinessCollection businessCollection)
        {
            if (businessCollection != null && businessCollection.Count > 0)
            {
                ConcurrentBag<Models.Audit> Audits = new ConcurrentBag<Models.Audit>();

                Parallel.ForEach(businessCollection.Cast<AUDEN.LogAudit>().ToList(), field =>
                 {
                     Audits.Add(ModelAssembler.CreateAudit(field));
                 });

                return Audits.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates the audits.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static List<AUDModel.Audit> CreateAudits(IDataReader reader)
        {
            if (reader != null)
            {
                ConcurrentBag<Models.Audit> Audits = new ConcurrentBag<Models.Audit>();
                while (reader.Read())
                {
                    try
                    {
                        var changes = JsonConvert.DeserializeObject<List<AuditChange>>((string)reader["Data"]);
                        Audits.Add(
                        new AUDModel.Audit
                        {
                            IsSerialize = (bool)reader["IsSerialize"],
                            ActionType = (AudictType?)(int)reader["TypeCode"],
                            RegisterDate = (DateTime)reader["RegisterDate"],
                            ObjectName = (string)reader["ObjectDescription"],
                            ColumName = (string)reader["ColumnName"],
                            ColumnDescription = (string)reader["ColumnDescription"],
                            Pk = reader["PK"]!= null ?JsonConvert.DeserializeObject<Dictionary<string, object>>(reader["PK"].ToString()) : new Dictionary<string, object>(0),
                            User = new User { Id = 0, Description = (string)reader[UniqueUsers.Properties.AccountName] },
                            Changes = changes
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                return Audits.ToList();
            }
            else
            {
                return null;
            }
        }

        #endregion
        #region paquetes
        public static List<AUDModel.Package> CreatePackages(List<AUDEN.LogPackage> LogPackages)
        {
            List<AUDModel.Package> packages = new List<AUDModel.Package>();
            if (LogPackages != null && LogPackages.Count > 0)
            {
                var config = MapperCache.GetMapper<AUDEN.LogPackage, AUDModel.Package>(cfg =>
                {
                    cfg.CreateMap<AUDEN.LogPackage, AUDModel.Package>();
                });
                return config.Map<List<AUDEN.LogPackage>, List<AUDModel.Package>>(LogPackages);
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region paquetes
        public static List<AUDModel.Entity> CreateEnties(List<AUDEN.LogEntity> LogEntities)
        {
            List<AUDModel.Entity> packages = new List<AUDModel.Entity>();
            if (LogEntities != null && LogEntities.Count > 0)
            {
                var config = MapperCache.GetMapper<AUDEN.LogEntity, AUDModel.Entity>(cfg =>
                {
                    cfg.CreateMap<AUDEN.LogEntity, AUDModel.Entity>();
                });
                return config.Map<List<AUDEN.LogEntity>, List<AUDModel.Entity>>(LogEntities);
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region leer Reglas
        public static AuditChange ReadObject(AuditChange auditChange)
        {
            if (auditChange != null)
            {
                TM.Task<string> taskValueBefore = null;
                TM.Task<string> taskValueAfter = null;

                var valueBefore = auditChange.ValueBefore.AsXml();
                var valueAfter = auditChange.ValueAfter.AsXml();
                if (valueBefore != null)
                {
                    taskValueBefore = TM.Task.Run(() =>
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(valueBefore.OuterXml);
                        var result = XmlHelperReader.GetRuleSetByXml(bytes);
                        DataFacadeManager.Dispose();
                        ConcurrentBag<dynamic> dataRule = ReadRuleHelper.LoadRules(result);
                        return JsonConvert.SerializeObject(dataRule);
                    });
                }
                if (valueAfter != null)
                {
                    taskValueAfter = TM.Task.Run(() =>
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(valueAfter.OuterXml);
                        var result = XmlHelperReader.GetRuleSetByXml(bytes);
                        DataFacadeManager.Dispose();
                        ConcurrentBag<dynamic> dataRule = ReadRuleHelper.LoadRules(result);
                        return JsonConvert.SerializeObject(dataRule);
                    });
                }

                try
                {
                    if (taskValueBefore == null && taskValueAfter != null)
                        TM.Task.WaitAll(new TM.Task[] { taskValueAfter });
                    else if (taskValueBefore != null && taskValueAfter != null)
                        TM.Task.WaitAll(new TM.Task[] { taskValueBefore, taskValueAfter });
                    else if (taskValueBefore != null && taskValueAfter == null)
                        TM.Task.WaitAll(new TM.Task[] { taskValueBefore });
                }
                catch (AggregateException ae)
                {
                    Debug.WriteLine(ae);
                    throw ae.Flatten();
                }
                auditChange.ValueBefore = taskValueBefore?.Result;
                auditChange.ValueAfter = taskValueAfter?.Result;
                return auditChange;
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Archivos Excel
         
       
        #endregion

    }
}
