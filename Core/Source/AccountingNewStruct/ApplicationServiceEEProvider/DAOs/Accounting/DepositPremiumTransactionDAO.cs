//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class DepositPremiumTransactionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region DepositPremiumTransactionDAO

        /// <summary>
        /// SaveDepositPremiumTransaction
        /// </summary>
        /// <param name="depositPremiumTransaction"></param>
        /// <param name="premiumReceivableCode"></param>
        /// <param name="payerTypeId"></param>
        /// <returns>DepositPremiumTransaction</returns>
        public DepositPremiumTransaction SaveDepositPremiumTransaction(DepositPremiumTransaction depositPremiumTransaction, int premiumReceivableCode, int payerTypeId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.DepositPremiumTransaction depositPremiumTransactionEntity = EntityAssembler.CreateDepositPremiumTransaction(depositPremiumTransaction, premiumReceivableCode, payerTypeId);
                
                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(depositPremiumTransactionEntity);

                return ModelAssembler.CreateDepositPremiumTransaction(depositPremiumTransactionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateDepositPremiumTransaction
        /// </summary>
        /// <param name="depositPremiumTransaction"></param>
        /// <param name="premiumReceivableCode"></param>
        /// <param name="payerTypeId"></param>
        /// <returns>DepositPremiumTransaction</returns>
        public DepositPremiumTransaction UpdateDepositPremiumTransaction(DepositPremiumTransaction depositPremiumTransaction, int premiumReceivableCode, int payerTypeId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.DepositPremiumTransaction.CreatePrimaryKey(depositPremiumTransaction.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.DepositPremiumTransaction depositPremiumTransactionEntity = (ACCOUNTINGEN.DepositPremiumTransaction)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                depositPremiumTransactionEntity.CollectCode = depositPremiumTransaction.Collect.Id;
                depositPremiumTransactionEntity.PremiumReceivableTransCode = premiumReceivableCode;
                depositPremiumTransactionEntity.PayerType = payerTypeId;
                depositPremiumTransactionEntity.PayerId = depositPremiumTransaction.Collect.Payer.IndividualId;
                depositPremiumTransactionEntity.RegisterDate = depositPremiumTransaction.Date;
                depositPremiumTransactionEntity.CurrencyCode = depositPremiumTransaction.Amount.Currency.Id;
                depositPremiumTransactionEntity.IncomeAmount = depositPremiumTransaction.Amount.Value;
                depositPremiumTransactionEntity.ExchangeRate = depositPremiumTransaction.ExchangeRate.SellAmount;
                depositPremiumTransactionEntity.Amount = depositPremiumTransaction.LocalAmount.Value;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(depositPremiumTransactionEntity);

                return ModelAssembler.CreateDepositPremiumTransaction(depositPremiumTransactionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDepositPremiumTransaction
        /// </summary>
        /// <param name="depositPremiumTransaction"></param>
        /// <returns>DepositPremiumTransaction</returns>
        public DepositPremiumTransaction GetDepositPremiumTransaction(DepositPremiumTransaction depositPremiumTransaction)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.DepositPremiumTransaction.CreatePrimaryKey(depositPremiumTransaction.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.DepositPremiumTransaction depositPremiumTransactionEntity = (ACCOUNTINGEN.DepositPremiumTransaction)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateDepositPremiumTransaction(depositPremiumTransactionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
