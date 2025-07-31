using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.PolicyCancellation
{
    public class PolicyCancellationModel
    {
        public decimal Amount { get; set; }
        public string Annul { get; set; }
        public int ApplicationTemporaryId { get; set; }
        public int ApplicationReceiptNumber { get; set; }
        public string AppliesCollection { get; set; }
        public string BranchDescription { get; set; }
        public int BranchId { get; set; }
        public int CancellationMark { get; set; }
        public DateTime CollectionDate { get; set; }
        public string CurrencyDescription { get; set; }
        public int CurrencyId { get; set; }
        public int EntryNumber { get; set; }
        public decimal ExchangeRate { get; set; }
        public int ExclusionAllowsToCancel { get; set; }
        public string ExclusionReason { get; set; }
        public int ExclusionReasonCode { get; set; }
        public int Id { get; set; }
        public string Insured { get; set; }
        public int InsuredId { get; set; }
        public string Intermediary { get; set; }
        public int IntermediaryId { get; set; }
        public int IntermediaryTypeId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime IssueDateFrom { get; set; }
        public DateTime IssueDateTo { get; set; }
        public decimal LocalAmount { get; set; }
        public int MotherPolicyId { get; set; }
        public string Observations { get; set; }
        public string Policy { get; set; }
        public int PolicyId { get; set; }
        public string PrefixDescription { get; set; }
        public int PrefixId { get; set; }
        public string Processed { get; set; }
        public int ProcessingMark { get; set; }
        public int ProcessNumber { get; set; }
        public DateTime QuoteDueDate { get; set; }
        public string Reprocess { get; set; }
        public string SalePointDescription { get; set; }
        public int SalePointId { get; set; }
    }
}