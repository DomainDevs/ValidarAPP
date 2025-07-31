//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class CoinsuranceCheckingAccountTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="tempCoinsuranceParentId"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public CoInsuranceCheckingAccountTransactionItem SaveCoinsuranceCheckingAccountTransactionItem(CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem, int imputationId, int tempCoinsuranceParentId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CoinsCheckingAccTrans coinsuranceCheckingAccountEntity = EntityAssembler.CreateCoinsuranceCheckingAccount(coinsuranceCheckingAccountTransactionItem, imputationId, tempCoinsuranceParentId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(coinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="coinsuranceParentId"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public CoInsuranceCheckingAccountTransactionItem UpdateCoinsuranceCheckingAccountTransactionItem(CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem, int coinsuranceParentId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CoinsCheckingAccTrans.CreatePrimaryKey(coinsuranceCheckingAccountTransactionItem.Id);

                //Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CoinsCheckingAccTrans coinsuranceCheckingAccountEntity = (ACCOUNTINGEN.CoinsCheckingAccTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                coinsuranceCheckingAccountEntity.CoinsuranceParentCode = coinsuranceParentId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(coinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="coInsuranceCheckingAccountTransactionItem"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public CoInsuranceCheckingAccountTransactionItem GetCoinsuranceCheckingAccountTransactionItem(CoInsuranceCheckingAccountTransactionItem coInsuranceCheckingAccountTransactionItem)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.CoinsCheckingAccTrans.CreatePrimaryKey(coInsuranceCheckingAccountTransactionItem.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CoinsCheckingAccTrans coinsuranceCheckingAccountEntity = (ACCOUNTINGEN.CoinsCheckingAccTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
