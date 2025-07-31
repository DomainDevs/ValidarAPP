using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable
{
    [KnownType("CheckPaymentOrderModel")]
    public class CheckPaymentOrderModel
    {
        public int Id { get; set; }
        public int StatusCheckBookControl { get; set; }
        public int CheckBookControlId { get; set; }
        public string CourierName { get; set; }
        public int CheckNumber { get; set; }
        public int AccountBankId { get; set; }
        public int IsCheckPrinted { get; set; }
        public int Status { get; set; }
        public int IndividualId { get; set; }
        public int BeneficiaryTypeId { get; set; }
        // Aumentado para BE
        public int BankId { get; set; }
        public string AccountBankNumber { get; set; }
        public List<PaymentOrderCheckModel> PaymentOrdersItems { get; set; }
    }

    [KnownType("PaymentOrderCheckModel")]
    public class PaymentOrderCheckModel
    {
        public int CheckBookControlId { get; set; }
        public int BeneficiaryTypeId { get; set; }
        public int IndividualId { get; set; }
        public int Status { get; set; }
        public int IsCheckPrinted { get; set; }
        public int PaymentOrderId { get; set; }
        public int AccountBankId { get; set; }
        public string PaymentDate { get; set; }
        public int TotalAmount { get; set; }
        public int TempImputationCode { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public int CheckNumber { get; set; }
        // Aumentado para BE
        public int BankId { get; set; }
        public string AccountBankNumber { get; set; }
    }
}