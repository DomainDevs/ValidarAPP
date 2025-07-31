using Sistran.Core.Application.AccountingServices.DTOs;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingPaymentBallotService
    {
        #region PaymenBallot

        /// <summary>
        /// SavePaymentBallot
        /// Graba Boletas de deposito
        /// </summary>
        /// <param name="paymentBallot"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentBallot</returns>
        [OperationContract]
        PaymentBallotDTO SavePaymentBallot(PaymentBallotDTO paymentBallot, int userId);

        /// <summary>
        /// BankBallotToDeposit
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>        
        /// <returns>List<PaymentBallotDTO></returns>
        [OperationContract]
        List<SearchDTO.PaymentBallotDTO> GetCheckBallots(int userId, int bankCode, string accountNumber, int branchId);

        /// <summary>
        /// GetCreditCardDepositBallots
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="branchId"></param>
        /// <returns>List<CreditCardPaymentBallotDTO></returns>
        [OperationContract]
        List<SearchDTO.CreditCardPaymentBallotDTO> GetCreditCardDepositBallots(int userId, int status, int creditCardTypeCode, int paymentTicketCode, int bankCode, string accountNumber, int branchId);

        [OperationContract]
        int GetTechnicalTransactionForPaymentBallotByPaymentCode(int paymentCode);

        #endregion

        #region PaymenTicketBallot

        /// <summary>
        /// SavePaymentTicketBallot
        /// </summary>
        /// <param name="paymentTicketBallotId"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="paymentBallotId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SavePaymentTicketBallot(int paymentTicketBallotId, int paymentTicketId, int paymentBallotId);

        /// <summary>
        /// ValidateExternalBallotDeposited
        /// Valida la existencia de una boleta de depósito.
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool ValidateExternalBallotDeposited(int paymentTicketCode);

        /// <summary>
        /// paymentBallotAccounting
        /// </summary>
        /// <param name="paymentBallotAccounting"></param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentBallotResponsesDTO> SaveAccountingPaymentBallot(PaymentBallotAccountingDTO paymentBallotAccounting);

       #endregion
    }
}
