// Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using PAYMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentRequestClaimDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentRequestClaim
        /// </summary>
        /// <param name="paymentRequestClaim"></param>
        public void SavePaymentRequestClaim(Array paymentRequestClaim)
        {
            try
            {
                // Graba PaymentRequestClaim
                PAYMEN.PaymentRequestClaim paymentRequestClaimEntity = EntityAssembler.CreatePaymentRequestClaim(paymentRequestClaim);

                // Validación para cuando venga de una solicitud de pago vario poner nulos en la tabla
                if (Convert.ToInt16(paymentRequestClaim.GetValue(5)) == -1 && Convert.ToInt16(paymentRequestClaim.GetValue(6)) == -1 &&
                    Convert.ToInt16(paymentRequestClaim.GetValue(7)) == -1 && Convert.ToInt16(paymentRequestClaim.GetValue(8)) == -1)
                {
                    paymentRequestClaimEntity.BranchCode = null;
                    paymentRequestClaimEntity.PrefixCode = null;
                    paymentRequestClaimEntity.PaymentTypeCode = null;
                    paymentRequestClaimEntity.ClaimNumber = null;
                }

                _dataFacadeManager.GetDataFacade().InsertObject(paymentRequestClaimEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePaymentRequest
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void UpdateClaimPaymentRequest(int paymentRequestId, int userId)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = PAYMEN.PaymentRequest.CreatePrimaryKey(paymentRequestId);

                // Encuentra el objeto en referencia a la llave primaria
                PAYMEN.PaymentRequest paymentRequestEntity = (PAYMEN.PaymentRequest)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentRequestEntity.ExecutedPaymentDate = DateTime.Now;
                paymentRequestEntity.StatusPayment = 1;
                paymentRequestEntity.UserIdPayment = userId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentRequestEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

    }
}
