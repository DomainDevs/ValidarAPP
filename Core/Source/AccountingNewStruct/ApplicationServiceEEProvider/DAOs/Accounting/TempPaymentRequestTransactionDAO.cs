//System
using System;
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonService.Models;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempPaymentRequestTransactionDAO
    {
        #region  Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods 

        /// <summary>
        /// GetTempPaymentRequestTransactions
        /// Valida si ya existe una determinada solicitud de pago en temporales o en reales
        /// basado en la solicitud de pago , el número de cuota, número de imputación 
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>List<TempClaimPayment/></returns>
        public List<ACCOUNTINGEN.TempPaymentRequestTrans> GetTempPaymentRequestTransactions(int paymentRequestId, int tempImputationId)
        {
            try
            {
                // Busca primero en temporales
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (tempImputationId > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentRequestTrans.Properties.TempApplicationCode, tempImputationId);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentRequestTrans.Properties.PaymentRequestCode, paymentRequestId);
                }

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempPaymentRequestTrans), criteriaBuilder.GetPredicate()));

                List<ACCOUNTINGEN.TempPaymentRequestTrans> tempPaymentRequestTrans= new List<ACCOUNTINGEN.TempPaymentRequestTrans>();
                foreach (ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestEntity in businessCollection.OfType<ACCOUNTINGEN.TempPaymentRequestTrans>())
                {
                    tempPaymentRequestTrans.Add(tempPaymentRequestEntity);
                }

                // Encontrado en temporales 
                return tempPaymentRequestTrans;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public bool DeleteTempPaymentRequestItem(int paymentRequestId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPaymentRequestTrans.CreatePrimaryKey(paymentRequestId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempPaymentRequestTrans paymentRequestTransEntity = (ACCOUNTINGEN.TempPaymentRequestTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (paymentRequestTransEntity != null)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(paymentRequestTransEntity);
                    return true;
                }

                return false;
            }
            catch (BusinessException)
            {
                return false;
            }
        }


        /// <summary>
        /// GetTempPaymentRequestTransactionByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>PaymentRequestTransaction</returns>
        public PaymentRequestTransaction GetTempPaymentRequestTransactionByTempImputationId(int tempImputationId)
        {
            decimal debits = 0;
            decimal credits = 0;

            PaymentRequestTransaction tempPaymentRequestTransaction = new PaymentRequestTransaction();
            tempPaymentRequestTransaction.Id = tempImputationId;

            List<ACCOUNTINGEN.TempPaymentRequestTrans> tempPaymentRequestTrans = GetTempPaymentRequestTrans(0, tempImputationId);

            foreach (ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestEntity in tempPaymentRequestTrans)
            {
                if (tempPaymentRequestEntity.Amount < 0)  // Si es negativo es crédito
                {
                    credits = credits + (Convert.ToDecimal(tempPaymentRequestEntity.Amount) * -1);
                }
                else
                {
                    debits = debits + Convert.ToDecimal(tempPaymentRequestEntity.Amount);
                }
            }

            tempPaymentRequestTransaction.TotalCredit = new Amount();
            tempPaymentRequestTransaction.TotalCredit.Value = credits;
            tempPaymentRequestTransaction.TotalDebit = new Amount();
            tempPaymentRequestTransaction.TotalDebit.Value = debits;

            return tempPaymentRequestTransaction;
        }

        /// <summary>
        /// GetTempPaymentRequestTrans
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns></returns>
        public List<ACCOUNTINGEN.TempPaymentRequestTrans> GetTempPaymentRequestTrans(int paymentRequestId, int tempImputationId)
        {
            try
            {
                List<ACCOUNTINGEN.TempPaymentRequestTrans> tempPaymentRequest= new List<ACCOUNTINGEN.TempPaymentRequestTrans>();

                // Busca primero en temporales
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (tempImputationId > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentRequestTrans.Properties.TempApplicationCode, tempImputationId);

                    if (paymentRequestId > 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.PaymentRequestCode, paymentRequestId);
                    }

                    // Asignamos BusinessCollection a una Lista
                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempPaymentRequestTrans), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestEntity in businessCollection.OfType<ACCOUNTINGEN.TempPaymentRequestTrans>())
                    {
                        tempPaymentRequest.Add(tempPaymentRequestEntity);
                    }
                }

                // Encontrado en temporales 
                return tempPaymentRequest;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SavePaymentRequestTransaction
        /// Graba pago de siniestro
        /// </summary>
        /// <param name="paymentRequestTransaction"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>bool</returns>
        public bool SavePaymentRequestTransaction(PaymentRequestTransactionItem paymentRequestTransaction, int imputationId, decimal exchangeRate)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentRequestTrans paymentRequestTransEntity = EntityAssembler.CreatePaymentRequestTrans(
                                                     paymentRequestTransaction, imputationId, exchangeRate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentRequestTransEntity);

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// DeletePaymentRequestTransactionByImputationId
        /// Borra una solicitud de pagos varios por imputación
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        public bool DeletePaymentRequestTransactionByImputationId(int imputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestTrans.Properties.ApplicationCode, imputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(ACCOUNTINGEN.PaymentRequestTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.PaymentRequestTrans paymentRequestTransEntity in businessCollection.OfType<ACCOUNTINGEN.PaymentRequestTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(paymentRequestTransEntity);
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// DeleteTempPaymentRequestByTempImputationId
        /// Borra registros de la solicitud de pagos varios por la imputacion
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPaymentRequestByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentRequestTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempPaymentRequestTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestTransEntity in businessCollection.OfType<ACCOUNTINGEN.TempPaymentRequestTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempPaymentRequestTransEntity);
                }

                return true;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

    }
}
