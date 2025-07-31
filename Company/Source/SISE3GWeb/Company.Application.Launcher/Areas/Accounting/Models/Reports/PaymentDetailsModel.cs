using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("PaymentDetailsModel")]
    public class PaymentDetailsModel
    {
        //Subinform Detalle Pagos
        public string Bill { get; set; }
        public int PaymentId { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        /*-------------------------------------*/
        public double IncomeAmount { get; set; }
        public double ExchangeRate { get; set; }
        public string Holder { get; set; }
        public string DocumentNumber { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
    }
}