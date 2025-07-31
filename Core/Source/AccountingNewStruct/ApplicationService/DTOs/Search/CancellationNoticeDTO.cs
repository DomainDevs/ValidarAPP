using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CancellationNoticeDTO
    {
        [DataMember]
        public DateTime CancellationDate { get; set; }

        [DataMember]
        public DateTime EffectiveDate { get; set; }

        [DataMember]
        public string Prefix { get; set; }

        [DataMember]
        public int PrefixCode { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string MovementTypeDescription { get; set; }

        [DataMember]
        public int CurrencyCode { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string PolicyNumber { get; set; }

        [DataMember]
        public int Sufix { get; set; }

        [DataMember]
        public string EndorsmentNumber { get; set; }

        [DataMember]
        public DateTime IssuingDate { get; set; }

        [DataMember]
        public DateTime ValidityDateFrom { get; set; }

        [DataMember]
        public DateTime ValidityDateTo { get; set; }

        [DataMember]
        public int AgentCode { get; set; }

        [DataMember]
        public string Agent { get; set; }

        [DataMember]
        public int InsuredCode { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public decimal TotalPrime { get; set; }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public int MotiveCode { get; set; }

        [DataMember]
        public int QuoteNumber { get; set; }

        [DataMember]
        public DateTime ExpirationDate { get; set; }

        [DataMember]
        public DateTime LastPaymentDate { get; set; }

        [DataMember]
        public string EndorsmentDescription { get; set; }

        [DataMember]
        public string PrefixTypeDescription { get; set; }

        [DataMember]
        public string AgentDocumentNumber { get; set; }

        [DataMember]
        public int DaysOfDelay { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Municipality { get; set; }

        [DataMember]
        public string RecoveryOfficial { get; set; }

        [DataMember]
        public string DepartmentManager { get; set; }

        [DataMember]
        public int BusinessLetter { get; set; }

        [DataMember]
        public string PathFile { get; set; }
    }
}
