//System
using System;
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempClaimPaymentRequestDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Public Methods

        /// <summary>
        /// GetPaymentRequestClaim
        /// Valida si ya existe una determinada solicitud de pago en temporales o en reales
        /// basado en la solicitud de pago , el número de cuota, número de imputación 
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="paymentNum"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="typePayment"></param>
        /// <returns>List<TempClaimPayment/></returns>
        public List<ACCOUNTINGEN.TempClaimPaymentReqTrans> GetTempClaimPayment(int paymentRequestId, int? paymentNum,
                                                                               int tempImputationId, int typePayment)
        {
            try
            {
                // Busca primero en temporales
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (tempImputationId > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.TempApplicationCode, tempImputationId).And();

                    // // Pago de liquidacion de siniestro
                    if (typePayment == 2)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.RequestType, PaymentRequestTypes.Payment);
                    }
                    if (typePayment == 0) // Saca todos
                    {
                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.RequestType, PaymentRequestTypes.Payment).Or(); //Siniestros falta ClaimsPaymentRequest
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.RequestType, PaymentRequestTypes.Void).Or();  //Salvamentos falta Salvage
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.RequestType, PaymentRequestTypes.Recovery).Or(); //Recobros
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.RequestType, PaymentRequestTypes.Payment);       //Varios
                        criteriaBuilder.CloseParenthesis();
                    }
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.PaymentRequestCode, paymentRequestId);

                    if (paymentNum != 0) // En caso de que sea solo liquidación de siniestro
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.PaymentNum, paymentNum);
                    }
                }

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempClaimPaymentReqTrans), criteriaBuilder.GetPredicate()));

                List<ACCOUNTINGEN.TempClaimPaymentReqTrans> tempClaimPayments = new List<ACCOUNTINGEN.TempClaimPaymentReqTrans>();
                foreach (ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPaymentEntity in businessCollection.OfType<ACCOUNTINGEN.TempClaimPaymentReqTrans>())
                {
                    tempClaimPayments.Add(tempClaimPaymentEntity);
                }

                // Encontrado en temporales 
                return tempClaimPayments;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempClaimPaymentRequestByTempImputationId
        /// Borra registros de la tabla TEMP_CLAIM_PAYMENT por la imputacion
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempClaimPaymentRequestByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.TempApplicationCode, tempImputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempClaimPaymentReqTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPaymentEntity in businessCollection.OfType<ACCOUNTINGEN.TempClaimPaymentReqTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempClaimPaymentEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteClaimsPaymentRequestItem
        /// Borra un registro de la tabla TEMP_CLAIM_PAYMENT por la llave primaria
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempClaimPaymentRequestItem(int claimsPaymentRequestId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempClaimPaymentReqTrans.CreatePrimaryKey(claimsPaymentRequestId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempClaimPaymentReqTrans claimPayment = (ACCOUNTINGEN.TempClaimPaymentReqTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (claimPayment != null)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(claimPayment);
                    return true;
                }

                return false;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}
