using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using accountsPayablesModel = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
  public class PaymentOrderAuthorizationDAO
    {
        #region Instance Variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SavePaymentOrderAuthorization
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public accountsPayablesModel.PaymentOrder SavePaymentOrderAuthorization(accountsPayablesModel.PaymentOrder paymentOrder, int authorizationLevel)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentOrderAuthorization paymentOrderAuthorizationEntity = EntityAssembler.CreateAuthorizationPaymentOrder(paymentOrder, authorizationLevel);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentOrderAuthorizationEntity);

                // Return del model
                return ModelAssembler.CreateAuthorizationPaymentOrder(paymentOrderAuthorizationEntity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        #endregion
    }
}
