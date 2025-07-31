//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentTaxDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentTax
        /// </summary>
        /// <param name="paymentTax"></param>
        /// <param name="paymentId"></param>
        /// <returns>PaymentTax</returns>
        public PaymentTax SavePaymentTax(PaymentTax paymentTax, int paymentId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentTax paymentTaxEntity = EntityAssembler.CreatePaymentTax(paymentTax, paymentId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentTaxEntity);

                // Return del model
                return ModelAssembler.CreatePaymentTax(paymentTaxEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePaymentTax
        /// Elimina un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="paymentTax"></param>
        /// <returns></returns>
        public void DeletePaymentTax(PaymentTax paymentTax)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentTax.CreatePrimaryKey(paymentTax.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentTax paymentTaxEntity = (ACCOUNTINGEN.PaymentTax)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentTaxEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
