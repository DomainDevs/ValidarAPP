
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    public class CollectDetailsModel
    {
        public string Bill { get; set; }
        public int BillItemCode { get; set; }
        public string Policy { get; set; }
        public string Endorsement { get; set; }
        public int Quota { get; set; }
        public double Amount { get; set; }
        public double ReceivedAmount { get; set; }
        public int PaymentId { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PayerName { get; set; }
    }
}