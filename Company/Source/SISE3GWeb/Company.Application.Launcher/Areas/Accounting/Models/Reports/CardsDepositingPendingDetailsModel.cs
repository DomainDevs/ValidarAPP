using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("CardsDepositingPendingDetailsModel")]
    public class CardsDepositingPendingDetailsModel
    {
        public int CreditCardTypeId { get; set; }
        public string CreditCardTypeName { get; set; }
        public int BranchCode { get; set; }
        public string BranchName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int UserCode { get; set; }
        public string UserName { get; set; }

        // Detalle
        public int BranchCodeDetail { get; set; }
        public string BranchNameDetail { get; set; }
        public int ReceiptNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CardDate { get; set; }
        public string CurrencyDescription { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal CardAmount { get; set; }
        public decimal CardTotal { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public string TechnicalTransaction { get; set; }
    }
}