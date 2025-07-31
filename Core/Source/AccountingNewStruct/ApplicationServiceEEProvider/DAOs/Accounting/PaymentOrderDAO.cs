//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Globalization;
using System.Threading;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentOrderDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #region PaymentOrder

        /// <summary>
        /// SavePaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrder SavePaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentOrder paymentOrderEntity = Assemblers.EntityAssembler.CreatePaymentOrder(paymentOrder);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentOrderEntity);

                // Return del model
                return ModelAssembler.CreatePaymentOrder(paymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>bool</returns>
        public bool UpdatePaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentOrder.CreatePrimaryKey(paymentOrder.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentOrder paymentOrderEntity = (ACCOUNTINGEN.PaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentOrderEntity.AccountBankCode = Convert.ToInt32(paymentOrder.BankAccountPerson.Id);
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

                if (paymentOrder.PaymentDate != new DateTime())
                {
                    paymentOrderEntity.PaymentDate = paymentOrder.PaymentDate;
                }
                
                paymentOrderEntity.PaymentSourceCode = paymentOrder.PaymentSource.Id;
                paymentOrderEntity.PayTo = paymentOrder.PayTo;
                paymentOrderEntity.PersonTypeCode = paymentOrder.PersonType.Id;

                if (paymentOrder.UserId > 0)
                {
                    if (paymentOrder.CancellationDate.ToString() == "01/01/0001 0:00:00" )
                    {
                        paymentOrderEntity.CancellationDate = null;
                    }
                    else
                    {
                        paymentOrderEntity.CancellationDate = paymentOrder.CancellationDate;
                    }
           
                    paymentOrderEntity.CancellationUserId = paymentOrder.UserId;
                }
                paymentOrderEntity.Status = paymentOrder.Status;
                paymentOrderEntity.Observation = paymentOrder.Observation;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentOrderEntity);

                // Return del model
                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        /// <summary>
        /// GetPaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrder GetPaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentOrder.CreatePrimaryKey(paymentOrder.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentOrder paymentOrderEntity = (ACCOUNTINGEN.PaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (paymentOrderEntity != null)
                {
                    // Return del model
                    return ModelAssembler.CreatePaymentOrder(paymentOrderEntity);
                }

                return null;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion




    }
}
