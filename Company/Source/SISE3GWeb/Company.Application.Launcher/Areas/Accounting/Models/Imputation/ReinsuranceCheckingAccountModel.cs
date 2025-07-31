using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    public class ReinsuranceCheckingAccountModel
    {
        public int ImputationId { get; set; }
        public List<ReinsuranceCheckingAccountItemModel> ReinsuranceCheckingAccountTransactionItems { get; set; }
    }

    [KnownType("ReinsuranceCheckingAccountItemModel")]
    public class ReinsuranceCheckingAccountItemModel
    {
        public int ReinsuranceCheckingAccountItemId { get; set; }
        public int BranchId { get; set; }
        public int SalePointId { get; set; }
        public int AccountingCompanyId { get; set; }
        public int LineBusinessId { get; set; }
        public int SubLineBusinessId { get; set; }
        public int AgentId { get; set; }
        public int ReinsuranceCompanyId { get; set; }
        public int IsFacultative { get; set; }
        public string SlipNumber { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractNumber { get; set; }
        public string Section { get; set; }
        public string Region { get; set; }
        public int CheckingAccountConceptId { get; set; }
        public int AccountingNature { get; set; }
        public int CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public int BillId { get; set; }
        public int ReinsuranceCheckingAccountId { get; set; }
        public int Period { get; set; }
        public int PolicyId { get; set; }
        public int EndorsementId { get; set; }
        public int ApplicationYear { get; set; }
        public int ApplicationMonth { get; set; }
        public List<ReinsuranceCheckingAccountItemModel> ReinsuranceCheckingAccountTransactionChild { get; set; }
    }
}