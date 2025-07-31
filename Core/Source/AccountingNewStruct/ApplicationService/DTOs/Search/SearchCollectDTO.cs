using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class SearchCollectDTO 
    {
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public int CollectStatus { get; set; }
        [DataMember]
        public double PaymentsTotal { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public DateTime? RegisterDate { get; set; }
        [DataMember]
        public DateTime? AccountingDate { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public string PayerDocumentNumber { get; set; }
        [DataMember]
        public string Payer { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int CollectControlCode { get; set; }
        [DataMember]
        public string CollectConceptDescription { get; set; }
        [DataMember]
        public string CollectDescription { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public double PostdatedValue { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string AccountingDateImputation { get; set; }
        [DataMember]
        public double PaymentsTotalImputation { get; set; }
        [DataMember]
        public int Rows { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int JournalEntryId { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public int ImputationId { get; set; }
        [DataMember]
        public int JournalEntryStatus { get; set; }
        [DataMember]
        public int AccountingTransaction { get; set; }
    }
}
