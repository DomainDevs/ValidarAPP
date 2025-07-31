using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("BillControlModel")]
    public class BillControlModel
    {
        public int BillControlId { get; set; }
        public List<BillControlPayment> BillControlPayments { get; set; }
    }

    [KnownType("BillControlPayment")]
    public class BillControlPayment
    {
        public int BillControlPaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public double PaymentTotalAdmitted { get; set; }
        public double PaymentsTotalReceived { get; set; }
        public double PaymentsTotalDifference { get; set; }
    }
}