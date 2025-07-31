namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class BankResponseCodesModel
    {
        public int Id { get; set; }
        public int BankNetworkId { get; set; }
        public int TableCode { get; set; }
        public string NetworkDescription { get; set; }
        public string RejectedCouponStatus { get; set; }
        public string AcceptedCouponStatus { get; set; }
    }
}