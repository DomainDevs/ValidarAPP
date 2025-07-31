//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentOrderBrokerAccountDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// PaymentOrderBrokerAccount
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <param name="brokerCheckingAccountId"></param>
        /// <returns>int</returns>
        public int SavePaymentOrderBrokerAccount(int paymentOrder, int brokerCheckingAccountId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentOrderBrokerAccount paymentOrderBrokerAccount = EntityAssembler.CreatePaymentOrderBrokerAccount(paymentOrder, brokerCheckingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentOrderBrokerAccount);

                // Return del model
                return paymentOrderBrokerAccount.PaymentOrderBrokerAccountId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
