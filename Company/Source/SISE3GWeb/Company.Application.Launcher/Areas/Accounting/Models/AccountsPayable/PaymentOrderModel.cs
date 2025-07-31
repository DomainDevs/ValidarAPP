using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable
{
    [KnownType("PaymentOrderModel")]
    public class PaymentOrderModel
    {
        public int PaymentOrderItemId { get; set; }
        public DateTime AccountingDate { get; set; }
        public int AccountBankId { get; set; }
        public int BranchId { get; set; }
        public int BranchPayId { get; set; }
        public int CompanyId { get; set; }
        public DateTime EstimatedPaymentDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int PaymentSourceId { get; set; }
        public int IndividualId { get; set; }
        public int PersonTypeId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal PaymentIncomeAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PayTo { get; set; }
        public int StatusId { get; set; }
        public string Observation { get; set; }
    }

    [KnownType("PaymentOrderModelList")]
    public class PaymentOrderModelList
    {
        public List<PaymentOrderModel> PaymentOrderItem { get; set; }
    }
}