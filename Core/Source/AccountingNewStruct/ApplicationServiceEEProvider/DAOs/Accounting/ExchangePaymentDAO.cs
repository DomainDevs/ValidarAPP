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
    internal class ExchangePaymentDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveExchangePayment
        /// Graba los item necesarios para la tabla ExchangePayment
        /// </summary>
        /// <param name="exchangePaymentId"></param>
        /// <param name="paymentSourceId"></param>
        /// <param name="paymentIdFinal"></param>
        public int SaveExchangePayment(int exchangePaymentId, int paymentSourceId, int paymentIdFinal)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ExchangePayment exchangePaymentEntity = EntityAssembler.CreateExchangePayment(exchangePaymentId, paymentSourceId, paymentIdFinal);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(exchangePaymentEntity);

                return exchangePaymentEntity.ExchangePaymentCode;
            }
            catch (ArgumentException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
