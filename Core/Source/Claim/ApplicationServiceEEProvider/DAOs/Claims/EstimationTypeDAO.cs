using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using System.Data;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;
using System.Linq;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class EstimationTypeDAO
    {
        public Prefix CreatePrefix(int estimationTypeId, Prefix prefix)
        {
            PARAMEN.EstimationTypePrefix entityEstimationTypePrefix = EntityAssembler.CreateEstimationTypePrefix(estimationTypeId, prefix);
            DataFacadeManager.Insert(entityEstimationTypePrefix);

            return ModelAssembler.CreateEstimationTypePrefix(entityEstimationTypePrefix);
        }

        public List<Prefix> GetPrefixesByEstimationTypeId(int estimationTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.EstimationTypePrefix.Properties.EstimateTypeCode, typeof(PARAMEN.EstimationTypePrefix).Name, estimationTypeId);

            return ModelAssembler.CreateEstimationsTypePrefix(DataFacadeManager.GetObjects(typeof(PARAMEN.EstimationTypePrefix), filter.GetPredicate()));
        }

        public void DeletePrefix(int estimationTypeId, int PrefixId)
        {
            PrimaryKey primaryKey = PARAMEN.EstimationTypePrefix.CreatePrimaryKey(estimationTypeId, PrefixId);
            DataFacadeManager.Delete(primaryKey);
        }

        public List<EstimationType> GetEstimationTypes()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();            
            filter.PropertyEquals(PARAMEN.EstimationType.Properties.Enabled, true);

            return ModelAssembler.CreateEstimationTypes(DataFacadeManager.GetObjects(typeof(PARAMEN.EstimationType), filter.GetPredicate()));
        }

        public EstimationType GetEstimationTypeById(int estimationTypeId)
        {
            PrimaryKey key = PARAMEN.EstimationType.CreatePrimaryKey(estimationTypeId);

            PARAMEN.EstimationType entityEstimationType = (PARAMEN.EstimationType)DataFacadeManager.GetObject(key);

            return ModelAssembler.CreateEstimationType(entityEstimationType);
        }

        #region Status

        public List<Status> GetStatusesByEstimationTypeId(int estimationTypeId)
        {
            ObjectCriteriaBuilder filterRelation = new ObjectCriteriaBuilder();
            filterRelation.PropertyEquals(PARAMEN.RelationTypeStatus.Properties.EstimateTypeCode, typeof(PARAMEN.RelationTypeStatus).Name, estimationTypeId);

            /// Lista de relacion entre estados y conceptos de estimación
            List<Status> relationTypeStatuses = ModelAssembler.CreateStatusesByEstimationType(DataFacadeManager.GetObjects(typeof(PARAMEN.RelationTypeStatus), filterRelation.GetPredicate()));

            List<Status> estimationTypeStatuses = new List<Status>();

            foreach (Status item in relationTypeStatuses)
            {
                PrimaryKey primaryKey = PARAMEN.EstimationTypeStatus.CreatePrimaryKey(item.Id);
                /* Traer objeto por primary key */
                PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = (PARAMEN.EstimationTypeStatus)DataFacadeManager.GetObject(primaryKey);

                Status estimationTypeStatus = ModelAssembler.CreateStatus(entityEstimationTypeStatus);

                /* Llenar descripcion del codigo interno */
                estimationTypeStatus.InternalStatus = GetInternalStatusById(entityEstimationTypeStatus.InternalStatusCode);

                estimationTypeStatuses.Add(estimationTypeStatus);
            }

            return estimationTypeStatuses;
        }

        public List<Status> GetEstimationTypeStatusUnassignedByEstimationTypeId(int estimationTypeId)
        {
            List<Status> statuses = GetStatuses();
            ObjectCriteriaBuilder filterRelation = new ObjectCriteriaBuilder();
            filterRelation.PropertyEquals(PARAMEN.RelationTypeStatus.Properties.EstimateTypeCode, typeof(PARAMEN.RelationTypeStatus).Name, estimationTypeId);

            /// Lista de relacion entre estados y conceptos de estimación
            List<Status> relationTypeStatuses = ModelAssembler.CreateStatusesByEstimationType(DataFacadeManager.GetObjects(typeof(PARAMEN.RelationTypeStatus), filterRelation.GetPredicate()));

            List<Status> estimationTypeStatuses = new List<Status>();

            foreach (Status item in relationTypeStatuses)
            {
                PrimaryKey primaryKey = PARAMEN.EstimationTypeStatus.CreatePrimaryKey(item.Id);
                /* Traer objeto por primary key */
                PARAMEN.EstimationTypeStatus entityEstimationTypeStatus = (PARAMEN.EstimationTypeStatus)DataFacadeManager.GetObject(primaryKey);

                Status estimationTypeStatus = ModelAssembler.CreateStatus(entityEstimationTypeStatus);

                /* Llenar descripcion del codigo interno */
                estimationTypeStatus.InternalStatus = GetInternalStatusById(entityEstimationTypeStatus.InternalStatusCode);

                estimationTypeStatuses.Add(estimationTypeStatus);
            }
            statuses.RemoveAll(x => estimationTypeStatuses.Any(y => y.Id == x.Id));
            return statuses;
        }



        public List<Status> GetStatuses()
        {
            List<Status> estimationTypeStatuses =
                ModelAssembler.CreateStatuses(DataFacadeManager.GetObjects(typeof(PARAMEN.EstimationTypeStatus)));
            return estimationTypeStatuses;
        }

        #endregion

        #region RelationTypeStatus

        public Status CreateStatusByEstimationType(Status status, int estimationTypeId)
        {
            PARAMEN.RelationTypeStatus entityRelationTypeStatus = EntityAssembler.CreateStatusByEstimationType(status, estimationTypeId);
            DataFacadeManager.Insert(entityRelationTypeStatus);

            return ModelAssembler.CreateStatusByEstimationType(entityRelationTypeStatus);
        }

        public void DeleteStatusByEstimationType(Status status, int estimationTypeId)
        {
            PrimaryKey primaryKey = PARAMEN.RelationTypeStatus.CreatePrimaryKey(status.Id, estimationTypeId);
            DataFacadeManager.Delete(primaryKey);
        }

        #endregion

        #region InternalStatus

        public InternalStatus GetInternalStatusById(int internalStatusId)
        {
            PrimaryKey primaryKey = PARAMEN.EstimationTypeInternalStatus.CreatePrimaryKey(internalStatusId);
            InternalStatus internalStatus =
                ModelAssembler.CreateInternalStatus((PARAMEN.EstimationTypeInternalStatus)DataFacadeManager.GetObject(primaryKey));
            return internalStatus;
        }

        public List<EstimationType> GetEstimationTypesByPrefixId(int prefixId)
        {
            List<EstimationType> estimationTypes = new List<EstimationType>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.EstimationTypePrefix.Properties.PrefixCode, prefixId);
            filter.And();
            filter.PropertyEquals(PARAMEN.EstimationType.Properties.Enabled, true);

            EstimationTypesView EstimationTypesView = new EstimationTypesView();
            ViewBuilder viewBuilder = new ViewBuilder("EstimationTypesView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, EstimationTypesView);

            if (EstimationTypesView.EstimationTypes.Count > 0)
            {
                estimationTypes = ModelAssembler.CreateEstimationsType(EstimationTypesView.EstimationTypes);
            }

            return estimationTypes;
        }

        #endregion

        #region Reason
        public Reason CreateReason(Reason reason)
        {
            PARAMEN.EstimationTypeStatusReason entityReason = EntityAssembler.CreateReason(reason);

            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);

            funtion.AddParameter(new Column(PARAMEN.EstimationTypeStatusReason.Properties.EstimationTypeStatusReasonCode));

            selectQuery.Table = new ClassNameTable(typeof(PARAMEN.EstimationTypeStatusReason), "ETS");
            selectQuery.AddSelectValue(new SelectValue(funtion));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    entityReason.EstimationTypeStatusReasonCode = (Convert.ToInt32(reader[0]) + 1);
                }
            }

            DataFacadeManager.Insert(entityReason);

            return ModelAssembler.CreateReason(entityReason);
        }

        public Reason UpdateReason(Reason reason)
        {
            PARAMEN.EstimationTypeStatusReason entityReason = EntityAssembler.CreateReason(reason);
            DataFacadeManager.Update(entityReason);

            return ModelAssembler.CreateReason(entityReason);
        }

        public void DeleteReason(int reasonId, int statusId, int prefixId)
        {
            PrimaryKey primaryKey = PARAMEN.EstimationTypeStatusReason.CreatePrimaryKey(reasonId, statusId, prefixId);
            DataFacadeManager.Delete(primaryKey);
        }

        public List<Reason> GetReasonsByStatusIdPrefixId(int statusId, int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.EstimationTypeStatusReason.Properties.EstimationTypeStatusCode, typeof(PARAMEN.EstimationTypeStatusReason).Name, statusId);
            filter.And();
            filter.PropertyEquals(PARAMEN.EstimationTypeStatusReason.Properties.PrefixCode, typeof(PARAMEN.EstimationTypeStatusReason).Name, prefixId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PARAMEN.EstimationTypeStatusReason), filter.GetPredicate());

            return ModelAssembler.CreateEstimationTypesStatusReasons(businessObjects);
        }

        public List<Reason> GetReasonsByPrefixId(int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.EstimationTypeStatusReason.Properties.PrefixCode, typeof(PARAMEN.EstimationTypeStatusReason).Name, prefixId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PARAMEN.EstimationTypeStatusReason), filter.GetPredicate());

            return ModelAssembler.CreateEstimationTypesStatusReasons(businessObjects);
        }

        public Reason GetReasonByReasonIdStatusIdPrefixId(int reasonId, int statusId, int prefixId)
        {
            PrimaryKey key = PARAMEN.EstimationTypeStatusReason.CreatePrimaryKey(reasonId, statusId, prefixId);
            return ModelAssembler.CreateReason((PARAMEN.EstimationTypeStatusReason)DataFacadeManager.GetObject(key));
        }
        #endregion

        #region AmountType
        public List<AmountType> GetAmountType()
        {
            return ModelAssembler.CreateAmountTypes(DataFacadeManager.GetObjects(typeof(Parameters.Entities.AmountType)));
        }

        public MinimumSalary GetMinimumSalaryByYear(int year)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.MinimumSalary.Properties.Year, typeof(COMMEN.MinimumSalary).Name);
            filter.Equal();
            filter.Constant(year);

            COMMEN.MinimumSalary entityMinimumSalary = DataFacadeManager.GetObjects(typeof(COMMEN.MinimumSalary), filter.GetPredicate()).Cast<COMMEN.MinimumSalary>().OrderByDescending(x => x.RegistrationDate).FirstOrDefault();
            
            return ModelAssembler.CreateMinimumSalaryByYear(entityMinimumSalary);
        }
        #endregion
    }
}
