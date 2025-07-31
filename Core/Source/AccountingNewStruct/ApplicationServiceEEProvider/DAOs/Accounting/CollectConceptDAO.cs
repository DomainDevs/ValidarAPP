using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
   public class CollectConceptDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCollectConcept
        /// </summary>
        /// <param name="collectConcept"></param>
        /// <returns>CollectConcept</returns>
        public CollectConcept SaveCollectConcept(CollectConcept collectConcept)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CollectConcept collectConceptEntity = EntityAssembler.CreateBillingConcept(collectConcept);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(collectConceptEntity);

                // Return del model
                return ModelAssembler.CreateCollectConcept(collectConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCollectConcept
        /// </summary>
        /// <param name="collectConcept"></param>
        /// <returns>CollectConcept</returns>
        public CollectConcept UpdateCollectConcept(CollectConcept collectConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CollectConcept.CreatePrimaryKey(collectConcept.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CollectConcept collectConceptEntity = (ACCOUNTINGEN.CollectConcept)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                collectConceptEntity.Description = collectConcept.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(collectConceptEntity);

                // Return del model
                return ModelAssembler.CreateCollectConcept(collectConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCollectConcept
        /// </summary>
        /// <param name="collectingConcept"></param>
        public void DeleteCollectConcept(CollectConcept collectingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CollectConcept.CreatePrimaryKey(collectingConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CollectConcept billingConceptEntity = (ACCOUNTINGEN.CollectConcept)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(billingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectConcepts
        /// </summary>
        /// <returns>List<CollectConcept/></returns>
        public List<CollectConcept> GetCollectConcepts()
        {
            try
            {
                // Asignamos BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectConcept)));

                // Return como lista
                return ModelAssembler.CreateCollectConcepts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
