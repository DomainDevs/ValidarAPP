using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("OtherPaymentsRequestReportModel")]
    public class OtherPaymentsRequestReportModel
    {
        public int PaymentRequestId { get; set; }
        public int Number { get; set; }
        public string EstimatedDate { get; set; }
        public int PersonTypeId { get; set; }
        public string PersonTypeDescription { get; set; }
        public int IndividualId { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
        public string RegistrationDate { get; set; }
        public decimal TotalAmountHeader { get; set; }
        public int UserId { get; set; }
        public string UserAccountName { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodDescription { get; set; }
        public string PaymentRequestDescription { get; set; }
        public int BillId { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherTypeDescription { get; set; }
        public string VoucherNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Taxes { get; set; }
        public decimal Retentions { get; set; }
    }
}