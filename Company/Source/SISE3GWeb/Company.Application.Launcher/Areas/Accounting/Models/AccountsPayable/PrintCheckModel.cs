using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable
{
    [KnownType("PrintCheckModel")]
    public class PrintCheckModel
    {
        public string AddressCompany { get; set; }
        public string Amount { get; set; }
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
        public int PersonTypeId { get; set; }
        public string CourierName { get; set; }
        public string RefoundDate { get; set; }
        public int DeliveryTypeId { get; set; }
        public string DescriptionCity { get; set; }
    }

    [KnownType("ListPrintCheckModel")]
    public class ListPrintCheckModel
    {
        public List<PrintCheckModel> PrintCheckModels { get; set; }
    }
}