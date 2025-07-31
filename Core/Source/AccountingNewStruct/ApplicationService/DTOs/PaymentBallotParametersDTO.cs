using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PaymentBallotParametersDTO
    {
        /// <summary>
        /// PaymentBallotId
        /// </summary>
        [DataMember]
        public int PaymentBallotId { get; set; }
        /// <summary>
        /// PaymentBallotNumber
        /// </summary>
        [DataMember]
        public string PaymentBallotNumber { get; set; }
        /// <summary>
        /// PaymentAccountNumber
        /// </summary>
        [DataMember]
        public string PaymentAccountNumber { get; set; }
        /// <summary>
        /// PaymentBallotBankId
        /// </summary>
        [DataMember]
        public int PaymentBallotBankId { get; set; }
        /// <summary>
        /// PaymentBallotAmount
        /// </summary>
        [DataMember]
        public decimal PaymentBallotAmount { get; set; }
        /// <summary>
        /// PaymentBallotBankAmount
        /// </summary>
        [DataMember]
        public decimal PaymentBallotBankAmount { get; set; }
        /// <summary>
        /// PaymentCurrency
        /// </summary>
        [DataMember]
        public int PaymentCurrency { get; set; }
        /// <summary>
        /// PaymentBankDate
        /// </summary>
        [DataMember]
        public DateTime PaymentBankDate { get; set; }
        /// <summary>
        /// PaymentStatus
        /// </summary>
        [DataMember]
        public int PaymentStatus { get; set; }
        /// <summary>
        /// PaymentTicketBallotModels
        /// </summary>
        [DataMember]
        public List<PaymentTicketBallotDTO> PaymentTicketBallotModels { get; set; }

        /// <summary>
        /// AccountingAccountId
        /// </summary>
        [DataMember]
        public int PaymentAccountingAccountId { get; set; }

    }
}
