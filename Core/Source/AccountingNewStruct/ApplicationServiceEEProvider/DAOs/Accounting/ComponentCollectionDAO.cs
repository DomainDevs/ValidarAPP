//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core.Application
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class ComponentCollectionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveComponentCollection
        /// </summary>
        /// <param name="componentCollectionId"></param>
        /// <param name="premiumReceivableId"></param>
        /// <param name="componentId"></param>
        /// <param name="amount"></param>
        /// <returns>int</returns>
        /// 
        public int SaveComponentCollection(int componentCollectionId, int premiumReceivableId, int componentId, Amount amount, ExchangeRate exchangeRate, Amount localAmount)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ComponentCollection componentCollectionEntity = EntityAssembler.CreateComponentCollection(componentCollectionId, premiumReceivableId, componentId, amount, exchangeRate, localAmount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(componentCollectionEntity);

                // Return del model
                return componentCollectionEntity.ComponentCollectionCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateComponentCollection
        /// </summary>
        /// <param name="componentCollectionId"></param>
        /// <param name="premiumReceivableId"></param>
        /// <param name="componentId"></param>
        /// <param name="amount"></param>
        /// <returns>bool</returns>
        public bool UpdateComponentCollection(int componentCollectionId, int premiumReceivableId, int componentId, Amount amount, ExchangeRate exchangeRate, Amount localAmount)
        {
            bool isUpdated = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ComponentCollection.CreatePrimaryKey(componentCollectionId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.ComponentCollection componentCollectionEntity = (ACCOUNTINGEN.ComponentCollection)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                
                componentCollectionEntity.PremiumReceivableTransCode = premiumReceivableId;
                componentCollectionEntity.ComponentId = componentId;
                componentCollectionEntity.IncomeAmount = amount.Value;
                componentCollectionEntity.ExchangeRate = exchangeRate.SellAmount;
                componentCollectionEntity.Amount = localAmount.Value;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(componentCollectionEntity);

                // Return del model
                isUpdated = true;
            }
            catch (BusinessException )
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// DeleteComponentCollection
        /// </summary>
        /// <param name="componentCollectionId"></param>
        /// <returns>bool</returns>
        public bool DeleteComponentCollection(int componentCollectionId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ComponentCollection.CreatePrimaryKey(componentCollectionId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.ComponentCollection componentCollectionEntity = (ACCOUNTINGEN.ComponentCollection)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(componentCollectionEntity);
                isDeleted = true;
            }
            catch (BusinessException )
            {
                isDeleted = false;
            }
            return isDeleted;
        }
    }
}
