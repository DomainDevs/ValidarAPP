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
    class PaymentTicketBallotDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentTicketBallot
        /// </summary>
        /// <param name="paymentTicketBallotId"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="paymentBallotId"></param>
        /// <returns>int</returns>
        /// 
        public int SavePaymentTicketBallot(int paymentTicketBallotId, int paymentTicketId, int paymentBallotId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentTicketBallot paymentTicketBallotEntity = EntityAssembler.CreatePaymenTicketBallot(paymentTicketBallotId, paymentTicketId, paymentBallotId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentTicketBallotEntity);

                // Return del model
                return paymentTicketBallotEntity.PaymentTicketBallotCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
    }
}
