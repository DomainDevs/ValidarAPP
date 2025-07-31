
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class BankAccountModel
    {
        public int BankAccountId { get; set; }
        public int AccountTypeCode { get; set; }
        public string AccountNumber { get; set; }
        public int CurrencyId { get; set; }
        public string AccountingNumber { get; set; }
        public int Enabled { get; set; }
        public string Description { get; set; }
    }
}