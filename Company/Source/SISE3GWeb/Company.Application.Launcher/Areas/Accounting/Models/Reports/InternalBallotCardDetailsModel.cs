using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    public class InternalBallotCardDetailsModel
    {
        public string PaymentTicketCode { get; set; }
        public int PaymentMethodTypeCode { get; set; }
        public string PaymentMethodTypeName { get; set; }
        public int CreditCardTypeCode { get; set; }
        public string CreditCardTypeName { get; set; }
        public int BranchCode { get; set; }
        public string BranchName { get; set; }
        public int ReceivingBankCode { get; set; }
        public string ReceivingBankName { get; set; }
        public string ReceivingAccountNumber { get; set; }
        public int ReceivingCurrencyCode { get; set; }
        public string ReceivingCurrencyName { get; set; }
        public int UserCode { get; set; }
        public string UserName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string TownName { get; set; }
        public double Amount { get; set; }
        public double CashAmount { get; set; }
        public double TaxAmount { get; set; }
        public double CommissionAmount { get; set; }

        // Detalle
        public int IssuingBankCode { get; set; }
        public string IssuingBankName { get; set; }
        public string VoucherNumber { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardDate { get; set; }
        public string AuthorizationNumber { get; set; }
        public string Holder { get; set; }
        public string CurrencySymbol { get; set; }
        public double IncomeAmount { get; set; }
        public double ExchangeRate { get; set; }
        public double CardAmount { get; set; }
        public double CardTaxAmount { get; set; }
        public double CardCommissionAmount { get; set; }
        public double TotalBallot { get; set; }
    }
}