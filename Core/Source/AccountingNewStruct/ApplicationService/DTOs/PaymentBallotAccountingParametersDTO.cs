using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
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

        [DataMember]
        public List<AccountingListParametersDTO> JournalEntryListParameters { get; set; }
    }
}
