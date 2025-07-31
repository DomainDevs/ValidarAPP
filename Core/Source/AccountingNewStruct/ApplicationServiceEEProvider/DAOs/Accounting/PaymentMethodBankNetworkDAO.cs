//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FWk
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentMethodBankNetworkDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SavePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public PaymentMethodBankNetwork SavePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentMethodBankNetwork paymentMethodBankNetworkEntity = EntityAssembler.CreatePaymentMethodBankNetwork(paymentMethodBankNetwork);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentMethodBankNetworkEntity);

                // Return del model
                return ModelAssembler.CreatePaymentMethodBankNetwork(paymentMethodBankNetworkEntity);
            }
            catch (DuplicatedObjectException)
            {
                // Se quiere grabar un registro duplicado de llave primaria
                paymentMethodBankNetwork.Id = -1;

                return paymentMethodBankNetwork;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public PaymentMethodBankNetwork UpdatePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentMethodBankNetwork.CreatePrimaryKey(paymentMethodBankNetwork.BankNetwork.Id, paymentMethodBankNetwork.PaymentMethod.Id, paymentMethodBankNetwork.BankAccountCompany.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentMethodBankNetwork paymentMethodBankNetworkEntity = (ACCOUNTINGEN.PaymentMethodBankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentMethodBankNetworkEntity.AccountBankCode = paymentMethodBankNetwork.BankAccountCompany.Id;
                paymentMethodBankNetworkEntity.Generate = paymentMethodBankNetwork.ToGenerate;
                paymentMethodBankNetworkEntity.Identifier = paymentMethodBankNetwork.BankNetwork.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentMethodBankNetworkEntity);

                // Return del model
                return ModelAssembler.CreatePaymentMethodBankNetwork(paymentMethodBankNetworkEntity);
            }
            catch (DuplicatedObjectException)
            {
                // Se quiere grabar un registro duplicado de llave primaria
                paymentMethodBankNetwork.Id = -1;

                return paymentMethodBankNetwork;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteSavePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        public void DeletePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentMethodBankNetwork.CreatePrimaryKey(paymentMethodBankNetwork.BankNetwork.Id, paymentMethodBankNetwork.PaymentMethod.Id, paymentMethodBankNetwork.BankAccountCompany.Id);

                ACCOUNTINGEN.PaymentMethodBankNetwork actionNetWorkEntity = (ACCOUNTINGEN.PaymentMethodBankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(actionNetWorkEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
  
    }
}
