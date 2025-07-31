using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using System.Collections;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempPaymentRequestClaimDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// GetTempPaymentRequestClaimByPaymentRequestId
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns>ArrayList</returns>
        public ArrayList GetTempPaymentRequestClaimByPaymentRequestId(int paymentRequestId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property("PaymentRequestCode");
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(paymentRequestId);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(PAYMEN.TempPaymentRequestClaim), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                ArrayList paymentRequestClaim = new ArrayList();
                foreach (PAYMEN.TempPaymentRequestClaim paymentRequestClaimEntity in businessCollection.OfType<PAYMEN.TempPaymentRequestClaim>())
                {
                    paymentRequestClaim.Add(paymentRequestClaimEntity);
                }
                return paymentRequestClaim;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPaymentRequestClaim
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        public void DeleteTempPaymentRequestClaim(int tempPaymentRequestId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(PAYMEN.TempPaymentRequestClaim.Properties.PaymentRequestCode, tempPaymentRequestId);

                //Obtiene la lista de registros a borrar
                BusinessCollection businessCollection = new BusinessCollection
                    (_dataFacadeManager.GetDataFacade().SelectObjects
                    (typeof(PAYMEN.TempPaymentRequestClaim), criteriaBuilder.GetPredicate()));

                foreach (PAYMEN.TempPaymentRequestClaim tempPaymentRequestClaimEntity in businessCollection.OfType<PAYMEN.TempPaymentRequestClaim>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(tempPaymentRequestClaimEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
