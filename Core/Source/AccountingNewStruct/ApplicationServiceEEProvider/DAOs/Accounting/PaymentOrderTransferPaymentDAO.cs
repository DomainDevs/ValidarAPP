//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentOrderTransferPaymentDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        ///<summary>
        /// SavePaymentOrderTransferPayment
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="transferPaymentOrderId"></param>
        /// <returns>bool</returns>
        public bool SavePaymentOrderTransferPayment(int paymentOrderId, int transferPaymentOrderId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentOrderTransferPayment paymentOrderTransferPaymentEntity = EntityAssembler.CreatePaymentOrderTransferPayment(paymentOrderId, transferPaymentOrderId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentOrderTransferPaymentEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        ///<summary>
        /// GetPaymentOrderTransferPaymentByPaymentOrderId
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>List<PaymentOrderTransferPayment/></returns>
        public List<ACCOUNTINGEN.PaymentOrderTransferPayment> GetPaymentOrderTransferPaymentByPaymentOrderId(int paymentOrderId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferPayment.Properties.PaymentOrderCode, paymentOrderId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PaymentOrderTransferPayment), criteriaBuilder.GetPredicate()));

                List<ACCOUNTINGEN.PaymentOrderTransferPayment> paymentOrderTransferPaymentList = new List<ACCOUNTINGEN.PaymentOrderTransferPayment>();

                if(businessCollection.Count > 0)
                {
                    paymentOrderTransferPaymentList.Add(businessCollection.OfType<ACCOUNTINGEN.PaymentOrderTransferPayment>().First());
                }

                return paymentOrderTransferPaymentList;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }
    }
}
