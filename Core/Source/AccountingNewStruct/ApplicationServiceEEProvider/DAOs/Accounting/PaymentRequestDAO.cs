using System;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sitran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentRequestDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SavePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest SavePaymentRequest(Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest paymentRequest)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentRequest paymentRequestEntity = EntityAssembler.CreatePaymentRequest(paymentRequest);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentRequestEntity);

                // Return del model
                return ModelAssembler.CreatePaymentRequest(paymentRequestEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdatePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest UpdatePaymentRequest(Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest paymentRequest)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentRequest.CreatePrimaryKey(paymentRequest.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentRequest paymentRequestEntity = (ACCOUNTINGEN.PaymentRequest)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (paymentRequest.Transaction != null)
                {
                    paymentRequestEntity.TechnicalTransaction = paymentRequest.Transaction.TechnicalTransaction;
                }
                else
                {
                    DateTime? estimatedPaymentDate;
                    DateTime? accountingDate;

                    if (paymentRequest.EstimatedDate == Convert.ToDateTime("01/01/0001 0:00:00"))
                    {
                        estimatedPaymentDate = null;
                    }
                    else
                    {
                        estimatedPaymentDate = paymentRequest.EstimatedDate;
                    }

                    if (paymentRequest.AccountingDate == Convert.ToDateTime("01/01/0001 0:00:00"))
                    {
                        accountingDate = null;
                    }
                    else
                    {
                        accountingDate = paymentRequest.AccountingDate;
                    }

                    paymentRequestEntity.PaymentRequestType = Convert.ToInt32(paymentRequest.PaymentRequestType);
                    paymentRequestEntity.Number = Convert.ToInt32(paymentRequest.PaymentRequestNumber);
                    paymentRequestEntity.ConceptSourceCode = Convert.ToInt32(paymentRequest.MovementType.ConceptSource.Id);
                    paymentRequestEntity.MovementTypeCode = paymentRequest.MovementType.Id;
                    paymentRequestEntity.CompanyCode = Convert.ToInt32(paymentRequest.Company.IndividualId);
                    paymentRequestEntity.BranchCode = Convert.ToInt32(paymentRequest.Branch.Id);
                    paymentRequestEntity.SalePointCode = Convert.ToInt32(paymentRequest.SalePoint.Id);
                    paymentRequestEntity.PersonTypeCode = Convert.ToInt32(paymentRequest.PersonType.Id);
                    paymentRequestEntity.BeneficiaryCode = Convert.ToInt32(paymentRequest.Beneficiary.IndividualId);
                    paymentRequestEntity.PaymentMethodTypeCode = Convert.ToInt32(paymentRequest.PaymentMethod.Id);
                    paymentRequestEntity.CurrencyCode = Convert.ToInt32(paymentRequest.Currency.Id);
                    paymentRequestEntity.TotalAmount = Convert.ToDecimal(paymentRequest.TotalAmount.Value);
                    paymentRequestEntity.EstimatedPaymentDate = estimatedPaymentDate;
                    paymentRequestEntity.AccountingDate = accountingDate;
                    paymentRequestEntity.RegisterDate = DateTime.Now;
                    paymentRequestEntity.Description = paymentRequest.Description;
                }

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentRequestEntity);

                // Return del model
                return ModelAssembler.CreatePaymentRequest(paymentRequestEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdatePaymentRequest
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest UpdatePaymentRequest(int paymentRequestId, int userId)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentRequest.CreatePrimaryKey(paymentRequestId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentRequest paymentRequestEntity = (ACCOUNTINGEN.PaymentRequest)
                           (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentRequestEntity.PaymentDate = DateTime.Now;
                paymentRequestEntity.PaymentStatus = 1;
                paymentRequestEntity.PaymentUserId = userId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentRequestEntity);

                // Return del model
                return ModelAssembler.CreatePaymentRequest(paymentRequestEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeletePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        public void DeletePaymentRequest(Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest paymentRequest)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentRequest.CreatePrimaryKey(paymentRequest.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentRequest paymentRequestEntity = (ACCOUNTINGEN.PaymentRequest)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentRequestEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest GetPaymentRequest(Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest paymentRequest)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentRequest.CreatePrimaryKey(paymentRequest.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentRequest paymentRequestEntity = (ACCOUNTINGEN.PaymentRequest)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreatePaymentRequest(paymentRequestEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentRequests
        /// </summary>
        /// <returns></returns>
        public List<Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.PaymentRequest> GetPaymentRequests()
        {
            try
            {
                //Se asigna una BussinesCollection a una lista.
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentRequest)));

                //Se retorna como una lista.
                return ModelAssembler.CreatePaymentRequests(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Get
    }
}
