using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("PaymentRequestItem")]
    public class PaymentRequestItem
    {
        public int TempPaymentCode { get; set; }
        public int TempImputationCode { get; set; }
        public int PaymentRequestCode { get; set; }
        public int BeneficiaryId { get; set; }
        public int CurrencyCode { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Amount { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EstimationDate { get; set; }
        public int BussinessType { get; set; }
        public int PaymentNumber { get; set; }
        public DateTime PaymentExpirationDate { get; set; }
        public int PaymentRequestNumber { get; set; }
    }
}