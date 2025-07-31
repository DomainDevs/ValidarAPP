using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.LegalPayment
{
    [KnownType("LegalPaymentModel")]
    public class LegalPaymentModel
    {
        public int LegalPaymentId { get; set; }
        public int RejectedPaymentId { get; set; }
        public DateTime LegalDate { get; set; }
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public DateTime DatePayment { get; set; }
        public string DocumentNumber { get; set; }
        public string IssuerName { get; set; }
        public int IssuingBankId { get; set; }
        public string IssuingAccountNumber { get; set; }
    }
}