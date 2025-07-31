using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.CancellationNotices
{
    [KnownType("CancellationNoticeModel")]
    public class CancellationNoticeModel
    {
        public DateTime CancellationDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Prefix { get; set; }
        public int PrefixCode { get; set; }
        public int BranchCode { get; set; }
        public string Branch { get; set; }
        public string Address { get; set; }
        public string MovementTypeDescription { get; set; }
        public int CurrencyCode { get; set; }
        public string Currency { get; set; }
        public string PolicyNumber { get; set; }
        public int Sufix { get; set; }
        public string EndorsmentNumber { get; set; }
        public DateTime IssuingDate { get; set; }
        public DateTime ValidityDateFrom { get; set; }
        public DateTime ValidityDateTo { get; set; }
        public int AgentCode { get; set; }
        public string Agent { get; set; }
        public int InsuredCode { get; set; }
        public string Insured { get; set; }
        public decimal TotalPrime { get; set; }
        public decimal Balance { get; set; }
        public int MotiveCode { get; set; }
        public int QuoteNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public string EndorsmentDescription { get; set; }
        public string PrefixTypeDescription { get; set; }
        public string AgentDocumentNumber { get; set; }
        public int DaysOfDelay { get; set; }
        public string Country { get; set; }
        public string Municipality { get; set; }
        public string RecoveryOfficial { get; set; }
        public string DepartmentManager { get; set; }
        public int BusinessLetter { get; set; }
    }
}