using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("DailyEntryReportModel")]
    public class DailyEntryReportModel
    {
        public string Branch { get; set; }
        
        public int AccountingAccountId { get; set; }
        
        public string AccountingAccountDescription { get; set; }
        
        public decimal AccountingNumber { get; set; }
        
        public int AccountingNature { get; set; }
        
        public int DailyEntryHeaderId { get; set; }
        
        public string DailyEntryHeaderDescription { get; set; }
        
        public int TransactionNumber { get; set; }
        
        public string Description { get; set; }
        
        public int EntryNumber { get; set; }
        
        public int Currency { get; set; }

        public string CurrencyDescription { get; set; }

        public int SalePoint { get; set; }
        
        public int DailyEntryId { get; set; }

        public int ImputationCode { get; set; }

        public string ImputationDescription { get; set; }

        public DateTime Date { get; set; }

        public DateTime ReceiptDate { get; set; }

        public int ReceiptNumber { get; set; }

        public int AccountingCompany { get; set; }

        public decimal AmountValue { get; set; }

        public string BankDescription { get; set; }

        public Decimal Debit { get; set; }
        
        public Decimal Credit { get; set; }
        
        public Decimal Balance { get; set; }

        public Decimal SubTotalDebit { get; set; }
        
        public Decimal SubTotalCredit { get; set; }
        
        public Decimal SubTotalBalance { get; set; }

        public string CompanyDescription { get; set; }
        public int TechnicalTransaction { get; set; }
    }
}