using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("EntryReportModel")]
    public class EntryReportModel
    {
        public string Branch { get; set; }

        public string AccountingDate { get; set; }
        
        public int AccountingAccountId { get; set; }
        
        public string AccountingAccountDescription { get; set; }

        public string AccountingAccountFullInformation { get; set; }
        
        public decimal AccountingNumber { get; set; }

        public int AccountingHeaderId { get; set; }
        
        public string ImputationDescription { get; set; }
        
        public int EntryNumber { get; set; }
        
        public int EntryId { get; set; }
        
        public string BankDescription { get; set; }
        
        public int ReceiptNumber { get; set; }
        
        public string Description { get; set; }
        
        public string ImputationCode { get; set; }
        
        public string CurrencyDescription { get; set; }

        public Decimal Debit { get; set; }
        
        public Decimal Credit { get; set; }
        
        public Decimal Balance { get; set; }

        public Decimal TotalDebit { get; set; }
        
        public Decimal TotalCredit { get; set; }
        
        public Decimal TotalBalance { get; set; }

        public Decimal TotalDebitInDate { get; set; }
        
        public Decimal TotalCreditInDate { get; set; }
        
        public Decimal TotalBalanceInDate { get; set; }
    }
}