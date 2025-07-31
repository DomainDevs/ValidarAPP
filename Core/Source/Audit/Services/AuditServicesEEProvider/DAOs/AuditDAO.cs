using Sistran.Core.Application.AuditServices.EEProvider.Assemblers;
using Sistran.Core.Application.AuditServices.EEProvider.Cache;
using Sistran.Core.Application.AuditServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using AUDEN = Sistran.Core.Application.Audit.Entities;
using AUDModel = Sistran.Core.Application.AuditServices.Models;
using USEREN = Sistran.Core.Application.UniqueUser.Entities;

using TM = System.Threading.Tasks;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AuditServices.DAOs
{

    /// <summary>
    /// Auditoria
    /// </summary>
    public class AuditDAO
    {
        /// <summary>
        /// Crear Auditorias
        /// </summary>
        /// <param name="audit">The audit.</param>
        public void CreateAudit(AUDEN.LogAudit audit)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(audit);
        }

        /// <summary>
        /// Gets the audit by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public async TM.Task<List<AUDModel.Audit>> GetAuditByFilter(Predicate filter)
        {
            BusinessCollection bussinesCollection = null;
            bussinesCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(AUDEN.LogAudit), filter);
            return await TM.Task.Run(() => ModelAssembler.CreateAudits(bussinesCollection));
        }

        /// <summary>
        /// Gets the audit by object.
        /// </summary>
        /// <param name="audit">The audit.</param>
        /// <returns></returns>
        public async TM.Task<List<AUDModel.Audit>> GetAuditByObject(AUDModel.Audit audit)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AUDEN.LogPackage.Properties.Id, "logPackage");
            filter.Equal();
            filter.Constant(audit.Package?.Id);
            if (audit.User?.Id != null && audit.User?.Id != 0)
            {
                filter.And();
                filter.Property(AUDEN.LogAudit.Properties.UserId, "log");
                filter.Equal();
                filter.Constant(audit.User?.Id);
            }
            if (audit.RegisterDate > DateTime.MinValue)
            {
                filter.And();
                filter.Property(AUDEN.LogAudit.Properties.RegisterDate, "log");
                filter.GreaterEqual();
                filter.Constant(audit.RegisterDate);
            }
            if (audit.CurrentTo > DateTime.MinValue)
            {
                filter.And();
                filter.Property(AUDEN.LogAudit.Properties.RegisterDate, "log");
                filter.LessEqual();
                filter.Constant(audit.CurrentTo);
            }
            if (audit.ObjectId != 0)
            {
                filter.And();
                filter.Property(AUDEN.LogAudit.Properties.ObjectCode, "log");
                filter.Equal();
                filter.Constant(audit.ObjectId);
            }
            if (audit.ActionType != null)
            {
                filter.And();
                filter.Property(AUDEN.LogAudit.Properties.TypeCode, "log");
                filter.Equal();
                filter.Constant(audit.ActionType);
            }
            return await TM.Task.Run(() =>
            {
                var result = GetObjects(filter);
                DataFacadeManager.Dispose();
                return result;
            }
            );
        }
        /// <summary>
        /// Gets the objects.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private List<AUDModel.Audit> GetObjects(ObjectCriteriaBuilder filter)
        {
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.RegisterDate, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.ObjectDescription, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.TypeCode, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.IsSerialize, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.Data, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.ColumnName, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.Pk, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(AUDEN.LogAudit.Properties.ColumnDescription, "log")));
            selectQuery.AddSelectValue(new SelectValue(new Column(USEREN.UniqueUsers.Properties.AccountName, "account")));
            Join join = new Join(new ClassNameTable(typeof(AUDEN.LogAudit), "log"), new ClassNameTable(typeof(AUDEN.LogEntity), "logEntity"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(AUDEN.LogAudit.Properties.ObjectCode, "log")
               .Equal().Property(AUDEN.LogEntity.Properties.Id, "logEntity").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(AUDEN.LogPackage), "logPackage"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(AUDEN.LogEntity.Properties.PackageCode, "logEntity")
             .Equal().Property(AUDEN.LogPackage.Properties.Id, "logPackage").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(USEREN.UniqueUsers), "account"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(AUDEN.LogAudit.Properties.UserId, "log")
                .Equal().Property(USEREN.UniqueUsers.Properties.UserId, "account").GetPredicate()
            };
            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();
            List<AUDModel.Audit> audits = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                audits = ModelAssembler.CreateAudits(reader);
            }
            if (audits != null && audits.Where(x => x.IsSerialize).Count() > 0)
            {
                audits = LoadSerialize(audits);
            }
            return audits;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        public static void GetEntity(String entityName)
        {
            if (!CacheManager.AuditEntities.ContainsKey(entityName))
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(AUDEN.LogEntity.Properties.EntityName, typeof(AUDEN.LogEntity).Name);
                filter.Equal();
                filter.Constant(entityName);
                var AuditEntity = DataFacadeManager.Instance.GetDataFacade().List<AUDEN.LogEntity>(filter.GetPredicate()).Cast<AUDEN.LogEntity>().FirstOrDefault();
                if (AuditEntity != null)
                {
                    CacheManager.AuditEntities.TryAdd(entityName, AuditEntity);
                }
                else
                {
                    EventLog.WriteEntry("Audit", $"No existe la entidad en la base: {entityName}", EventLogEntryType.Error);
                }

            }
        }

        /// <summary>
        /// Gets the packages.
        /// </summary>
        /// <returns></returns>
        public async TM.Task<List<AUDModel.Package>> GetPackages()
        {
            List<AUDEN.LogPackage> PackageEntity = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AUDEN.LogPackage.Properties.IsEnabled, typeof(AUDEN.LogPackage).Name);
            filter.Equal();
            filter.Constant(true);

            PackageEntity = DataFacadeManager.Instance.GetDataFacade().List<AUDEN.LogPackage>(filter.GetPredicate()).Cast<AUDEN.LogPackage>().ToList();

            return await TM.Task.Run(() => ModelAssembler.CreatePackages(PackageEntity));
        }

        /// <summary>
        /// Gets the entities by description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public async TM.Task<List<AUDModel.Entity>> GetEntitiesByDescription(string description, int idPackage)
        {
            List<AUDEN.LogEntity> logEntity = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AUDEN.LogEntity.Properties.Description, typeof(AUDEN.LogEntity).Name);
            filter.Like();
            filter.Constant("%" + description + "%");
            filter.And();
            filter.Property(AUDEN.LogEntity.Properties.PackageCode, typeof(AUDEN.LogEntity).Name);
            filter.Equal();
            filter.Constant(idPackage);

            logEntity = DataFacadeManager.Instance.GetDataFacade().List<AUDEN.LogEntity>(filter.GetPredicate()).Cast<AUDEN.LogEntity>().ToList();

            return await TM.Task.Run(() => ModelAssembler.CreateEnties(logEntity));
        }

        /// <summary>
        /// Loads the serialize.
        /// </summary>
        /// <param name="audits">The audits.</param>
        /// <returns></returns>
        private List<AUDModel.Audit> LoadSerialize(List<AUDModel.Audit> audits)
        {

            TM.Parallel.ForEach(audits.Where(x => x.ColumName.Trim() != string.Empty), z =>
            {
                try
                {
                    if (z.Changes != null)
                    {
                        var result = ModelAssembler.ReadObject(z.Changes.Where(x => x.Id == z.ColumName).FirstOrDefault());
                        if (result != null)
                        {
                            z.Changes.Clear();
                            z.Changes.Add(result);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(Errors.ErrorCreateRule + e.GetBaseException().Message);
                }

            });
            return audits;
        }

    }
}