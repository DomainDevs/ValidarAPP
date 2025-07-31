//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class OtherPaymentsRequestDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        ///<summary>
        /// SaveOtherPaymentsRequest
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns>bool</returns>
        public bool SaveOtherPaymentsRequest(int collectId, int paymentRequestId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.OtherPaymentsRequest otherPaymentsRequestEntity = EntityAssembler.CreateOtherPaymentsRequest(collectId, paymentRequestId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(otherPaymentsRequestEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// SavePaymentRequestVarious
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public bool SavePaymentRequestVarious(int collectId, int paymentRequestId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentRequestVarious paymentRequestVariousEntity = EntityAssembler.CreatePaymentRequestVarious(collectId, paymentRequestId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentRequestVariousEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

    }
}
