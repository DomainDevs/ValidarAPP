using Newtonsoft.Json.Linq;
using Sistran.Core.Application.AuditServices.DAOs;
using Sistran.Core.Application.AuditServices.EEProvider.Assemblers;
using Sistran.Core.Application.AuditServices.EEProvider.Cache;
using Sistran.Core.Application.AuditServices.EEProvider.Resources;
using Sistran.Core.Application.AuditServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Audit;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using AUDEN = Sistran.Core.Application.Audit.Entities;
using TR = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Application.AuditServices.EEProvider
{
    public static class LoadAudit
    {

        /// <summary>
        /// Gets or sets the audit queue.
        /// </summary>
        /// <value>
        /// The audit queue.
        /// </value>
        private static string AuditQueue { get; set; }
        /// <summary>
        /// Initializes the <see cref="LoadAudit" /> class.
        /// </summary>
        public static void AuditLog()
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuditQueue"]))
            {
                AuditQueue = ConfigurationManager.AppSettings["AuditQueue"];
            }
            else
            {
                AuditQueue = "AuditQueue";
            }
            TR.Task.Run(() =>
            {
                GetEntities();
            }
            );

        }

        /// <summary>
        /// Audits the data.
        /// </summary>
        public static void AuditData(object body)
        {
            try
            {
                object objBody = body;
                if (body is string)
                {
                    objBody = JObject.Parse(body.ToString()).ToObject(typeof(AuditData));
                }
                else if (body as JObject != null)
                {
                    objBody = ((JObject)body).ToObject(typeof(AuditData));
                   
                }               
                if (objBody != null)
                {
                    LoadData(objBody);
                }
                else
                {
                    EventLog.WriteEntry("Audit", Errors.ErrorRabbit + (AuditData)((JObject)body).ToObject(typeof(AuditData)), EventLogEntryType.Error);
                    throw new Exception(Errors.ErrorObject);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $"Errors.ErrorRabbit {ex.GetBaseException().Message}", EventLogEntryType.Error);
            }


        }
        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="body">The body.</param>
        private static void LoadData(object body)
        {

            var auditData = (AuditData)body;

            if (auditData != null)
            {
                ConcurrentBag<Models.AuditChange> ConcurrentBag = new ConcurrentBag<Models.AuditChange>();
                AuditDAO auditDAO = new AuditDAO();
                AuditDAO.GetEntity(auditData.Object);
                var audit = EntityAssembler.CreateAudit(auditData);
                if (audit != null)
                {
                    if (audit.TypeSerializeCode == null)
                        audit.TypeSerializeCode = (int)AudictSerializeType.Xml;
                    {
                    }
                    auditDAO.CreateAudit(audit);
                }
                else
                {
                    throw new Exception(Errors.ErrorInvalidFormat);
                }
            }
        }
        /// <summary>
        /// Gets the entities.
        /// </summary>
        private static void GetEntities()
        {
            BusinessCollection Audits = null;
            Audits = DataFacadeManager.Instance.GetDataFacade().List(typeof(AUDEN.LogEntity), null);
            if (Audits != null)
            {
                TR.Parallel.ForEach(Audits.Cast<AUDEN.LogEntity>(), audit =>
               {
                   CacheManager.AuditEntities.TryAdd(audit.EntityName, audit);
               }, ParallelHelper.DebugParallelFor());
            }
        }
    }
}
