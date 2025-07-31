using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("ClaimsPaymentRequestItem")]
    public class ClaimsPaymentRequestItem
    {
        public int TempClaimPaymentCode { get; set; }
        public int TempImputationCode { get; set; }
        public int PaymentRequestCode { get; set; }
        public int ClaimCode { get; set; }
        public int BeneficiaryId { get; set; }
        public int CurrencyCode { get; set; }
        public double IncomeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public double Amount { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EstimationDate { get; set; }
        public int BussinessType { get; set; }
        public int RequestType { get; set; }
        public int PaymentNum { get; set; }
        public DateTime PaymentExpirationDate { get; set; }
        public int PaymentRequestNumber { get; set; }
    }
}