//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;

//Sistran FWk
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class PaymentTicketItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentTicketItem
        /// Graba un nuevo registro en la tabla ACC.PAYMENT_TICKET_ITEM
        /// </summary>
        /// <param name="paymentTicketItemId"></param>
        /// <param name="paymentId"></param>
        /// <param name="paymentTicketId"></param>
        /// <returns>bool</returns>
        public bool SavePaymentTicketItem(int paymentTicketItemId, int paymentId, int paymentTicketId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentTicketItem paymentTicketItemEntity = EntityAssembler.CreatePaymentTicketItem(paymentTicketItemId, paymentId, paymentTicketId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentTicketItemEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        /// <summary>
        /// DeletePaymentTicketItem
        /// Elimina un registro de la tabla ACC.PAYMENT_TICKET_ITEM
        /// </summary>
        /// <param name="paymentTicketItemId"></param>
        public void DeletePaymentTicketItem(int paymentTicketItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentTicketItem.CreatePrimaryKey(paymentTicketItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentTicketItem paymentTicketItemEntity = (ACCOUNTINGEN.PaymentTicketItem)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentTicketItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        /// <summary>
        /// GetPaymentTicketsByPaymentTicketCode
        /// </summary>
        /// <param name="paymentTickedCode"></param>
        /// <returns></returns>
        public PaymentTicket GetPaymentTicketsByPaymentTicketCode(int paymentTickedCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTickedCode);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentTicketItem), criteriaBuilder.GetPredicate()));

                return ModelAssembler.CreatePaymentTicketItem(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
    }
}
