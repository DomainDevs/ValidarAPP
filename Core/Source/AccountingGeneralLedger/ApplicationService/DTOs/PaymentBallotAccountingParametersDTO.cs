using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class PaymentBallotAccountingParametersDTO
    {
        [DataMember]
        public PaymentBallotDTO PaymentBallot { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string ReceiptNumber { get; set; }

        [DataMember]
        public DateTime ReceiptDate { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public CheckBallotAccountingParameterDTO CheckBallotAccountingParameters { get; set; }
        
        /// <summary>
        /// JournalEntryListParameters
        /// </summary>
        [DataMember]
        public List<JournalEntryListParametersDTO> JournalEntryListParameters { get; set; }
    }
}
