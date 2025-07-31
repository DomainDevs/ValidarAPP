
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.AccountReclassification
{
    public class ReclassificationModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int BranchId { get; set; }
        public string BranchDescription { get; set; }
        public string SourceAccountingAccountNumber { get; set; }
        public string SourceAccountingAccountDescription { get; set; }
        public string DestinationAccountingAccountNumber { get; set; }
        public string DestinationAccountingAccountDescription { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
        public string Nature { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenterDescription { get; set; }
        public int AnalysisId { get; set; }
        public string AnalysisDescription { get; set; }
        public int AnalysisConceptId { get; set; }
        public string AnalysisConceptDescription { get; set; }
    }
}