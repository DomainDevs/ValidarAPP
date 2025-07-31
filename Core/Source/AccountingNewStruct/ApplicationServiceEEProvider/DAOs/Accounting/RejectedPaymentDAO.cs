//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using System;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class RejectedPaymentDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveRejectedPayment
        /// </summary>
        /// <param name="rejectedPayment"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>RejectedPayment</returns>
        public RejectedPayment SaveRejectedPayment(Models.RejectedPayment rejectedPayment, int userId, DateTime registerDate)
        {
            try
            {
               
                // Convertir de model a entity
                ACCOUNTINGEN.RejectedPayment rejectedPaymentEntity = EntityAssembler.CreateRejectedPayment(rejectedPayment, userId, registerDate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(rejectedPaymentEntity);

                // Return del model
                return ModelAssembler.CreateRejectedPayment(rejectedPaymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
