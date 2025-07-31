using System;
using System.Configuration;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempPaymentOrderDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #region TempPaymentOrder

        /// <summary>
        /// SaveTempPaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrder SaveTempPaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempPaymentOrder paymentOrderEntity = EntityAssembler.CreateTempPaymentOrder(paymentOrder);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentOrderEntity);

                // Return del model
                return ModelAssembler.CreateTempPaymentOrder(paymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempPaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrder UpdateTempPaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPaymentOrder.CreatePrimaryKey(paymentOrder.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempPaymentOrder paymentOrderEntity = (ACCOUNTINGEN.TempPaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                
                paymentOrderEntity.AccountBankCode = paymentOrder.BankAccountPerson.Id;
                paymentOrderEntity.AccountingDate = paymentOrder.AccountingDate;
                paymentOrderEntity.Amount = paymentOrder.LocalAmount.Value;
                paymentOrderEntity.BranchCdPay = paymentOrder.BranchPay.Id;
                paymentOrderEntity.BranchCode = paymentOrder.Branch.Id;
                paymentOrderEntity.CompanyCode = paymentOrder.Company.IndividualId;
                paymentOrderEntity.CurrencyCode = paymentOrder.Amount.Currency.Id;
                paymentOrderEntity.ExchangeRate = paymentOrder.ExchangeRate.BuyAmount;
                paymentOrderEntity.IncomeAmount = paymentOrder.Amount.Value;
                paymentOrderEntity.IndividualId = paymentOrder.Beneficiary.IndividualId;
                paymentOrderEntity.EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate;
                paymentOrderEntity.PaymentMethodCode = paymentOrder.PaymentMethod.Id;
                paymentOrderEntity.PaymentSourceCode = paymentOrder.PaymentSource.Id;
                paymentOrderEntity.PayTo = paymentOrder.PayTo;
                paymentOrderEntity.PersonTypeCode = paymentOrder.PersonType.Id;
                paymentOrderEntity.RegisterDate = DateTime.Now;
                paymentOrderEntity.Status = paymentOrder.Status;
                paymentOrderEntity.Observation = paymentOrder.Observation;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentOrderEntity);

                // Return del model
                return ModelAssembler.CreateTempPaymentOrder(paymentOrderEntity);
            }
            catch (BusinessException)
            {
                throw new BusinessException(ConfigurationManager.AppSettings["UnhandledExceptionMsj"]);
            }
        }

        /// <summary>
        /// DeleteTempPaymentOrder
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPaymentOrder(int paymentOrderId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPaymentOrder.CreatePrimaryKey(paymentOrderId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempPaymentOrder paymentOrderEntity = (ACCOUNTINGEN.TempPaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentOrderEntity);

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }
        
        #endregion
    }
}
