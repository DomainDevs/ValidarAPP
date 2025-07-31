using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using System.Data;
using System.Linq;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    internal class PriorityRetentionDAO
    {
        #region Instance variables
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #endregion

        #region Get
        
        public List<PriorityRetention> GetPriorityRetentions()
        {
            // Asignar BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.PriorityRetention)));
            // Retornar el model
            return ModelAssembler.CreatePriorityRetentions(businessCollection);
        }

        public List<PriorityRetention> GetPriorityRetentionsByPrefixCd(int prefixCd)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.PriorityRetention.Properties.PrefixCode, typeof(REINSEN.PriorityRetention).Name, prefixCd);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.PriorityRetention), filter.GetPredicate());
            return ModelAssembler.CreatePriorityRetentions(businessObjects);
        }

        public List<PriorityRetentionDetail> GetPriorityRetentionDetailsByPriorityRetentionId(int priorityRetentionId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.PriorityRetentionDetail.Properties.PriorityRetentionId, typeof(REINSEN.PriorityRetentionDetail).Name, priorityRetentionId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.PriorityRetentionDetail), filter.GetPredicate());
            return ModelAssembler.CreatePriorityRetentionDetails(businessObjects);
        }

        public List<PriorityRetentionDetail> GetPriorityRetentionDetailsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.PriorityRetentionDetail.Properties.PolicyId, typeof(REINSEN.PriorityRetentionDetail).Name, policyId).And();
            filter.PropertyEquals(REINSEN.PriorityRetentionDetail.Properties.EndorsementId, typeof(REINSEN.PriorityRetentionDetail).Name, endorsementId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.PriorityRetentionDetail), filter.GetPredicate());
            return ModelAssembler.CreatePriorityRetentionDetails(businessObjects);
        }

        #endregion

        #region Save
        public void SavePriorityRetention(PriorityRetention priorityRetention)
        {
            // Convertir de model a entity
            REINSEN.PriorityRetention entityPriorityRetention = EntityAssembler.CreatePriorityRetention(priorityRetention);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityPriorityRetention);
        }

        public void SavePriorityRetentionDetail(PriorityRetentionDetail priorityRetentionDetail)
        {
            
            // Convertir de model a entity
            REINSEN.PriorityRetentionDetail entityPriorityRetentionDetail = EntityAssembler.CreatePriorityRetentionDetail(priorityRetentionDetail);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityPriorityRetentionDetail);
        }

        #endregion

        #region Update

        public void UpdatePriorityRetention(PriorityRetention priorityRetention)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.PriorityRetention.CreatePrimaryKey(priorityRetention.Id);
            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.PriorityRetention entityPriorityRetention = (REINSEN.PriorityRetention)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            entityPriorityRetention.PrefixCode = priorityRetention.Prefix.Id;
            entityPriorityRetention.PriorityRetentionAmount = priorityRetention.PriorityRetentionAmount;
            entityPriorityRetention.ValidityFrom = priorityRetention.ValidityFrom;
            entityPriorityRetention.ValidityTo = priorityRetention.ValidityTo;
            entityPriorityRetention.Enabled = priorityRetention.Enabled;
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityPriorityRetention);
        }
        #endregion

        #region Delete

        public bool DeletePriorityRetention(int priorityRetentionId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.PriorityRetention.CreatePrimaryKey(priorityRetentionId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.PriorityRetention entityPriorityRetention = (REINSEN.PriorityRetention)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityPriorityRetention);
            return true;

        }
        #endregion


    }
}
