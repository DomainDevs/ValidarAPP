using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("RejectedCheckReportModel")]
    public class RejectedCheckReportModel
    {
        public string Id { get; set; }
        public string Place { get; set; }
        public string Payer { get; set; }
        public string CheckNumber { get; set; }
        public string IssuerBank { get; set; }
        public string DateCheck { get; set; }
        public string ReceiverCheck { get; set; }
        public string ReceiverBank { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }
        public string Commission { get; set; }
        public string TaxCommission { get; set; }
        public string Motive { get; set; }

        public string Branch { get; set; }
        public string Prefix { get; set; }
        public string Policy { get; set; }
        public string Endorsement { get; set; }
        public string Quote { get; set; }
        public decimal Amount { get; set; }


        //Voucher
        public string CreditCardDescription { get; set; }
        public string Voucher { get; set; }
        public int CreditCardTypeCode { get; set; }
        public int PaymentMethodTypeCode { get; set; }
    }
}