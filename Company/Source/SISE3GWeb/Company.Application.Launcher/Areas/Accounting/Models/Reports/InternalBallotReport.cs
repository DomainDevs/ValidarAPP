using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    public class InternalBallotReport //: IReportDto
    {
        // Cabecera
        public string PaymentTicketCode { get; set; }
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
        public double CashAmount { get; set; }
        public double Amount { get; set; }

        // Detalle
        public int IssuingBankCode { get; set; }
        public string IssuingBankName { get; set; }
        public string IssuingAccountNumber { get; set; }
        public string CheckNumber { get; set; }
        public string Holder { get; set; }
        public string CurrencySymbol { get; set; }
        public double IncomeAmount { get; set; }
        public double ExchangeRate { get; set; }
        public double CheckAmount { get; set; }
        public double TotalBallot { get; set; }

    }
}