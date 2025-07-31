
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.AccountReclassification
{
    public class AccountingReclassificationModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int AccountingAccountOriginId { get; set; }
        public string AccountingAccountOrigin { get; set; }
        public int AccountingAccountDestinationId { get; set; }
        public string AccountingAccountDestination { get; set; }
        public string PrefixOpening { get; set; }
        public string BranchOpening { get; set; }
    }
}