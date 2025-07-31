using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("CoinsuranceCheckingAccountModel")]
    public class CoinsuranceCheckingAccountModel
    {
        public int ImputationId { get; set; }
        public List<CoinsuranceCheckingAccountItemModel> CoinsuranceCheckingAccountTransactionItems { get; set; }
    }

    [KnownType("CoinsuranceCheckingAccountItemModel")]
    public class CoinsuranceCheckingAccountItemModel
    {
        public int CoinsuranceCheckingAccountItemId { get; set; }
        public int BranchId { get; set; }
        public int SalePointId { get; set; }
        public int AccountingCompanyId { get; set; }
        public int CoinsuranceType { get; set; }
        public int CoinsuredCompanyId { get; set; }
        public int CheckingAccountConceptId { get; set; }
        public int AccountingNatureId { get; set; }
        public string AccountingDate { get; set; }
        public int CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public int BillId { get; set; }
        public int CoinsuranceCheckingAccountId { get; set; }
        public List<CoinsuranceCheckingAccountItemModel> CoinsuranceCheckingAccountTransactionChild { get; set; }
    }
}