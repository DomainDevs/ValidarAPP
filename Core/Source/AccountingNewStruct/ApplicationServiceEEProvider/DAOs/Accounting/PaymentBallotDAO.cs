//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class PaymentBallotDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentBallot
        /// </summary>
        /// <param name="paymentBallot"></param>
        /// <param name="registerDate"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentBallot</returns>
        public PaymentBallot SavePaymentBallot(PaymentBallot paymentBallot, DateTime registerDate, int userId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentBallot paymentBallotEntity = EntityAssembler.CreatePaymenBallot(paymentBallot, registerDate, userId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentBallotEntity);

                // Return del model
                return ModelAssembler.CreatePaymentBallot(paymentBallotEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


    }
}
