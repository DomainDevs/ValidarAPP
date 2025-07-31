//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class CheckPaymentOrderTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SavePaymentOrderCheckPaymentTransactionItem
        /// </summary>
        /// <param name="checkPaymentOrderId"></param>
        /// <param name="paymentOrderId"></param>
        /// <returns>bool</returns>
        public bool SavePaymentOrderCheckPaymentTransactionItem(int checkPaymentOrderId, int paymentOrderId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentOrderCheckPayment paymentOrderCheckPaymentEntity = EntityAssembler.CreatePaymentOrderCheckPayment(checkPaymentOrderId, paymentOrderId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentOrderCheckPaymentEntity);

                // Return del model
                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
