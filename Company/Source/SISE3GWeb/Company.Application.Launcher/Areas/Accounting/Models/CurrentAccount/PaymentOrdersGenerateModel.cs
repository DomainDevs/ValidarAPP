using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CurrentAccount
{
    [KnownType("PaymentOrdersGenerateModel")]
    public class PaymentOrdersGenerateModel
    {
        //Cabecera
        public int Id { get; set; }
        public string ProcessNumber { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime EstimatedDatePayment { get; set; }

        //Detalle
        public string PaymentOrderNumber { get; set; }
        public string AgentDocumentNumber { get; set; }
        public string AgentName { get; set; }
        public string AgentDocNumberName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
    }
}