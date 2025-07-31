using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.SpecialProcess
{
    [KnownType("AmortizationItemModel")]
    public class AmortizationItemModel
    {
        public int ProcessId { get; set; }
        public int ImputationId { get; set; }
        public int AmortizationItemId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int PrefixId { get; set; }
        public string PrefixName { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public int EndorsementNumber { get; set; }
        public int InsuredId { get; set; }
        public string InsuredDocumentNumber { get; set; }
        public string InsuredName { get; set; }
        public int InsuredPersonTypeId { get; set; }
        public int AgentId { get; set; }
        public string AgentDocumentNumber { get; set; }
        public string AgentName { get; set; }
        public int AgentPersonTypeId { get; set; }
        public int PayerId { get; set; }
        public string PayerDocumentNumber { get; set; }
        public string PayerName { get; set; }
        public int PayerPersonTypeId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Exchange { get; set; }
        public decimal Amount { get; set; }
        public int ImputationReceiptNumber { get; set; }
    }

    [KnownType("ItemsToAppliedModel")]
    public class ItemsAmortizationToAppliedModel
    {
        public List<AmortizationItemModel> AmortizationItem { get; set; }
    }
    
}