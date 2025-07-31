namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{

    public class FinancialPlan
    {
        public int BranchId { get; set; }

        public int PrefixId { get; set; }

        public decimal PolicyNumber { get; set; }

        public int FinancialEndorsement { get; set; }

        public int FinancialPayer { get; set; }      

    }
}