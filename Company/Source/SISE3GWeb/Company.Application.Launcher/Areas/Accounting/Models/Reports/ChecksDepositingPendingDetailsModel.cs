using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("ChecksDepositingPendingDetailsModel")]
    public class ChecksDepositingPendingDetailsModel
    {
        public int IssuingBankCode { get; set; }
        public string IssuingBankName { get; set; }
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
        public string IssuingAccountNumber { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal CheckAmount { get; set; }
        public decimal CheckTotal { get; set; }
        public string StatusDescription { get; set; }
        public int TechnicalTransaction { get; set; }
    }
}