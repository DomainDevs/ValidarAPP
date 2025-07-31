using System;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempPremiumReceivableTransactionItemDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        ///<summary>
        /// SaveTempPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public PremiumReceivableTransactionItem SaveTempPremiumRecievableTransactionItem(PremiumReceivableTransactionItem premiumRecievableTransactionItem, int imputationId, decimal exchangeRate, int userId, DateTime registerDate)
        {
            try
            {
                // Se converte de modelo a entidad
                ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumRecievableEntity = EntityAssembler.CreateTempPremiumReceivable(premiumRecievableTransactionItem, imputationId, exchangeRate, userId, registerDate);

                _dataFacadeManager.GetDataFacade().InsertObject(tempPremiumRecievableEntity);

                return ModelAssembler.CreateTempPremiumReceivableTransactionItem(tempPremiumRecievableEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempPremiumReceivable
        /// </summary>
        /// <param name="premiumReceivableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public PremiumReceivableTransactionItem UpdateTempPremiumReceivableTransactionItem(PremiumReceivableTransactionItem premiumReceivableTransactionItem, int imputationId, decimal exchangeRate, int userId, DateTime registerDate)
        {
            try
            {
                int? statusQuota = 0;

                if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount)
                {
                    statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaPartial; //PARCIAL
                }
                if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
                {
                    statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaTotal; //TOTAL
                }

                // Se Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPremiumReceivableTrans.CreatePrimaryKey(premiumReceivableTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivableEntity = (ACCOUNTINGEN.TempPremiumReceivableTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempPremiumReceivableEntity.TempApplicationCode = imputationId;
                tempPremiumReceivableEntity.PolicyId = premiumReceivableTransactionItem.Policy.Id;
                tempPremiumReceivableEntity.EndorsementId = premiumReceivableTransactionItem.Policy.Endorsement.Id;
                tempPremiumReceivableEntity.PaymentNum = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number;
                tempPremiumReceivableEntity.PaymentAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount;
                tempPremiumReceivableEntity.PayerId = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId;
                tempPremiumReceivableEntity.IncomeAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                tempPremiumReceivableEntity.CurrencyCode = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id;
                tempPremiumReceivableEntity.ExchangeRate = exchangeRate;
                tempPremiumReceivableEntity.Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount * exchangeRate;
                tempPremiumReceivableEntity.RegisterDate = registerDate;
                tempPremiumReceivableEntity.DiscountedCommission = premiumReceivableTransactionItem.DeductCommission.Value;
                tempPremiumReceivableEntity.PremiumReceivableQuotaStatusCode = statusQuota;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempPremiumReceivableEntity);

                // Return del model
                return ModelAssembler.CreateTempPremiumReceivableTransactionItem(tempPremiumReceivableEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPremiumRecievableTransactionItem(int tempTransactionItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPremiumReceivableTrans.CreatePrimaryKey(tempTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivableEntity = (ACCOUNTINGEN.TempPremiumReceivableTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (tempPremiumReceivableEntity != null)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempPremiumReceivableEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
