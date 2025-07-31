using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class BankAccountPersonModel 
    {
        public int BankAccountCode { get; set; }
        
        public int AccountTypeCode { get; set; }
        
        public string AccountTypeDescription { get; set; }
        
        public string Description { get; set; }
        
        public int IndividualId { get; set; }
        
        public string DocumentNumber { get; set; }

        public string Number { get; set; }
        
        public string AccountName { get; set; }
        
        public int BankCode { get; set; }
        
        public string BankDescription { get; set; }
        
        public bool Enabled { get; set; }
        
        public bool Default { get; set; }
        
        public int CurrencyCode { get; set; }

        public string CurrencyDescription { get; set; }
        
        public int AccountingAccountCode { get; set; }
        
        public string AccountingNumber { get; set; }
        
        public string AccountingName { get; set; }
        
        public string DisabledDate { get; set; }
        
        public int BranchCode { get; set; }
        
        public string BranchDescription { get; set; }
    }
}
