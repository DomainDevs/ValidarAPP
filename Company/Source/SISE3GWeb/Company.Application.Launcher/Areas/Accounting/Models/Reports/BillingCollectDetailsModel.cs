using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("BillingCollectDetailsModel")]
    public class BillingCollectDetailsModel
    {
        public int CollectCode { get; set; }
        public string Description { get; set; }
        public string AccountNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string CurrencyDescription { get; set; }
        public decimal Amount { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public string PaymentDescription { get; set; }
        public int PayerId { get; set; }
        public string PayerName { get; set; }
        public int Status { get; set; }
    }
}