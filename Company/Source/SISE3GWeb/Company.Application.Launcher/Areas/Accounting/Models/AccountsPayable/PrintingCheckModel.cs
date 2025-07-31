using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable
{
    [KnownType("PrintingCheckModel")]
    public class PrintingCheckModel
    {
        public string AddressCompany { get; set; }
        public decimal Amount { get; set; }
        public string BeneficiaryName { get; set; }
        public int BranchId { get; set; }
        public int CheckNumber { get; set; }
        public string CompanyName { get; set; }
        public string EstimatedPaymentDate { get; set; }
        public int NumberPaymentOrder { get; set; }
        public int PaymentSourceId { get; set; }
        public int AccountBankId { get; set; }
        public string BankName { get; set; }
        public string AccountCurrentNumber { get; set; }
        public string CurrencyName { get; set; }
        public int CheckPaymentOrderCode { get; set; }
        public string DescriptionCity { get; set; }
    }
}