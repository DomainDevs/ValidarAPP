using Sistran.Core.Application.AccountingServices.DTOs;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using SearchDTO=Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingPaymentTicketService
    {
        #region PaymentTicket

        /// <summary>
        /// SaveInternalBallot
        /// Graba un nuevo boleta interna
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        [OperationContract]
        PaymentTicketDTO SaveInternalBallot(PaymentTicketDTO paymentTicket, int userId);

        /// <summary>
        /// UpdateInternalBallot
        /// Actualiza boleta interna
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>        
        /// <returns>PaymentTicket</returns>
        [OperationContract]
        PaymentTicketDTO UpdateInternalBallot(PaymentTicketDTO paymentTicket, int userId);


        /// <summary>
        /// UpdateInternalBallot
        /// Actualiza boleta interna
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>        
        /// <returns></returns>
        [OperationContract]
        void DeleteInternalBallot(PaymentTicketDTO paymentTicket, int userId);

        /// <summary>
        /// ValidateCashAmount
        /// Valida que el importe en efectivo ingresado no sea mayor total del efectivo recibido por caja menos el efectivo que ya esté asociado a 
        /// otras boletas internas del mismo usuario y fecha de caja abierta
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currencyId"></param>
        /// <param name="userId"></param>
        /// <param name="cashAmountAdmitted"></param>
        /// <param name="registerDate"></param>
        /// <returns>0 si el valor está permitida, en caso contrario el valor máximo que se permite</returns>
        [OperationContract]
        decimal ValidateCashAmount(int branchId, int currencyId, int userId, decimal cashAmountAdmitted, string registerDate, int paymentTicketId);


        /// <summary>
        /// GetPaymentTicketItemsByCollectId
        /// 
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<PaymentTicketItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.PaymentTicketItemDTO> GetPaymentTicketItemsByCollectId(int collectId);

        /// <summary>
        /// CancelInternalBallot
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int CancelInternalBallot(PaymentTicketDTO paymentTicket, int userId);



        #endregion

        #region Search Internal Ballot


        /// <summary>
        /// GetChecksToDepositBallot
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="currencyCode"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="checkNumber"></param>
        /// <param name="userId"></param>
        /// <returns>List<CheckToDepositInternalBallotDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckToDepositInternalBallotDTO> GetChecksToDepositBallot(int branchCode, int paymentMethodTypeCode, int currencyCode, int issuingBankCode, int checkNumber, int userId);


        /// <summary>
        /// GetReportInternalBallot
        /// Obtiene la cabecera y detalle de una boleta interna para la impresión
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="uiviewName"></param>
        /// <returns>List<InternalBallotReportDTO/></returns>
        [OperationContract]
        List<SearchDTO.InternalBallotReportDTO> GetReportInternalBallot(int userId, int paymentTicketCode, string uiviewName);

        /// <summary>
        /// GetInternalBallotNotDeposited
        /// Obtiene las boletas internas no depositadas. Cuando el parámetro es 0 se obtiene los números de boleta interna y el resultado se coloca 
        /// en el combo. Si es > 0 se obtiene la cabecera de la boleta interna.
        /// </summary> 
        /// <param name="internalBallotNumber"></param>
        /// <returns>List<BallotNotDepositedDTO/></returns>
        [OperationContract]
        List<SearchDTO.BallotNotDepositedDTO> GetInternalBallotNotDeposited(int internalBallotNumber);


        /// <summary>
        /// GetDetailInternalBallotNotDeposited
        /// Obtiene el detalle de una boleta interna, para ello se filtra por el número de boleta, estado o número de cheque (opcional)
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="checkDate"></param>
        /// <returns>List<DetailBallotNotDepositedDTO/></returns>
        [OperationContract]
        List<SearchDTO.DetailBallotNotDepositedDTO> GetDetailInternalBallotNotDeposited(int branchCode, int paymentTicketCode, int paymentStatus, DateTime checkDate);

        /// <summary>
        /// GetPaymentTicketItemsByPaymentCode        
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <returns>List<PaymentTicketItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.PaymentTicketItemDTO> GetPaymentTicketItemsByPaymentTicketCode(int paymentTicketCode);

        /// <summary>
        /// SearchInternalBallotCard
        /// Obtiene las boletas internas de depósito de tarjetas en base a los criterios de búsqueda
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="creditCardTypeId"></param>
        /// <param name="branchId"></param>
        /// <returns>List<SearchInternalBallotCardDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchInternalBallotCardDTO> SearchInternalBallotCard(int bankId, string startDate, string endDate, int paymentTicketId, int creditCardTypeId, int branchId);

        /// <summary>
        /// DepositSlipSearch
        /// Obtiene los boletas internas tanto de cheques como de efectivo
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentTicketId"></param>        
        /// <returns>List<SearchInternalBallotDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchInternalBallotDTO> DepositSlipSearch(int bankId, string startDate, string endDate, int paymentTicketId);

        /// <summary>
        /// GetDetailChecks
        /// Obtiene el detalle de los cheques 
        /// </summary>
        /// <param name="paymentTicketId"></param>        
        /// <returns>List<DetailCheckDTO/></returns>
        [OperationContract]
        List<SearchDTO.DetailCheckDTO> GetDetailChecks(int paymentTicketId);

        /// <summary>
        /// GetDetailCards
        /// </summary>
        /// <param name="paymentTicketId"></param>        
        /// <returns>List<DetailCardDTO/></returns>
        [OperationContract]
        List<SearchDTO.DetailCardDTO> GetDetailCards(int paymentTicketId);

        #endregion

        #region Internal Ballot Card

        /// <summary>
        /// GetCardsToDepositBallot
        /// Obtiene las tarjetas acreditables y no acreditables a depositar en una boleta interna
        /// </summary>         
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="currencyCode"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="voucherNumber"></param>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [OperationContract]
        List<SearchDTO.CardToDepositInternalBallotDTO> GetCardsToDepositBallot(int paymentMethodTypeCode, int currencyCode, int issuingBankCode, int voucherNumber, int creditCardTypeCode, int branchCode, DateTime toDate);

        /// <summary>
        /// GetReportInternalBallotCard
        /// Obtiene la cabecera y detalle de una boleta interna para la impresión
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="uiviewName"></param>
        /// <returns>List<InternalBallotCardReportDTO/></returns>
        [OperationContract]
        List<SearchDTO.InternalBallotCardReportDTO> GetReportInternalBallotCard(int userId, int paymentTicketCode, string uiviewName);

        /// <summary>
        /// GetCreditableHeaderCard
        /// Obtiene la cabecera de tarjeta de crédito acreditables y no acreditable
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <returns>CreditableCardHeaderDTO</returns>
        [OperationContract]
        SearchDTO.CreditableCardHeaderDTO GetCreditableHeaderCard(int paymentTicketCode, int paymentMethodTypeCode);

        /// <summary>
        /// GetCreditableDetailCard
        /// Obtiene el detalle de la boleta interna de depósito de tarjetas acreditables y no acreditables
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <returns>List<CardToDepositInternalBallotDTO/></returns>
        [OperationContract]
        List<SearchDTO.CardToDepositInternalBallotDTO> GetCreditableDetailCard(int paymentTicketCode, int paymentMethodTypeCode);

        #endregion

        /// <summary>
        /// Get next payment ticket sequence value
        /// </summary>
        /// <returns>Identifier</returns>
        [OperationContract]
        int GetPaymentTicketSequence();
    }
}
