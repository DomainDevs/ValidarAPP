//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PrefixComponentCollectionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SavePrefixComponentCollection
        /// </summary>
        /// <param name="prefixComponentCollectionId"></param>
        /// <param name="componentCollectionId"></param>
        /// <param name="lineBusinessId"></param>
        /// <param name="subLineBusinessId"></param>
        /// <param name="amount"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="localAmount"></param>
        /// <returns>int</returns>
        public int SavePrefixComponentCollection(int prefixComponentCollectionId, int componentCollectionId, int lineBusinessId, int subLineBusinessId, Amount amount, ExchangeRate exchangeRate, Amount localAmount)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollectionEntity = EntityAssembler.CreatePrefixComponentCollection(prefixComponentCollectionId, componentCollectionId, lineBusinessId, subLineBusinessId, amount, exchangeRate, localAmount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(prefixComponentCollectionEntity);

                // Return del model
                return prefixComponentCollectionEntity.PrefixComponentCollectionCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePrefixComponentCollection
        /// </summary>
        /// <param name="prefixComponentCollectionId"></param>
        /// <param name="componentCollectionId"></param>
        /// <param name="lineBusinessId"></param>
        /// <param name="subLineBusinessId"></param>
        /// <param name="amount"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="localAmount"></param>
        /// <returns>bool</returns>
        public bool UpdatePrefixComponentCollection(int prefixComponentCollectionId, int componentCollectionId, int lineBusinessId, int subLineBusinessId, Amount amount, ExchangeRate exchangeRate, Amount localAmount)
        {
            bool isUpdated = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PrefixComponentCollection.CreatePrimaryKey(prefixComponentCollectionId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollectionEntity = (ACCOUNTINGEN.PrefixComponentCollection)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                prefixComponentCollectionEntity.ComponentCollectionCode = componentCollectionId;
                prefixComponentCollectionEntity.LineBusinessCode = lineBusinessId;
                prefixComponentCollectionEntity.SubLineBusinessCode = subLineBusinessId;
                prefixComponentCollectionEntity.IncomeAmount = amount.Value;
                prefixComponentCollectionEntity.ExchangeRate = exchangeRate.SellAmount;
                prefixComponentCollectionEntity.Amount = localAmount.Value;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(prefixComponentCollectionEntity);

                // Return del model
                isUpdated = true;
            }
            catch (BusinessException)
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// DeletePrefixComponentCollection
        /// </summary>
        /// <param name="prefixComponentCollectionId"></param>
        /// <returns>bool</returns>
        public bool DeletePrefixComponentCollection(int prefixComponentCollectionId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PrefixComponentCollection.CreatePrimaryKey(prefixComponentCollectionId);

                //realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollectionEntity = (ACCOUNTINGEN.PrefixComponentCollection)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(prefixComponentCollectionEntity);
                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

    }
}
