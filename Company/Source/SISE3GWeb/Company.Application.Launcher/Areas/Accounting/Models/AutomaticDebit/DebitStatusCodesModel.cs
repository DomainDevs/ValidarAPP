
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class DebitStatusCodesModel
    {
        public int Id { get; set; }
        public string SmallDescription { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsRetry { get; set; }
        public string DebitStatusType { get; set; }
        public int RetryDays { get; set; }
    }
}