
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class CompanyBankAccountModel
    {
        public int CompanyBankAccountId { get; set; }
        public int BankId { get; set; }
        public int BranchId { get; set; }
        public int AccountTypeId { get; set; }
        public int CurrencyId { get; set; }
        public string AccountNumber { get; set; }
        public string Description { get; set; }
        public int Enabled { get; set; }
        public int Default { get; set; }
        public string DisabledDate { get; set; }
        public int AccountingAccountId { get; set; }
        public string AccountingAccountNumber { get; set; }
        public string AccountingAccountName { get; set; }
    }
}