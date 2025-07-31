using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.PaymentOrders
{
    [KnownType("PaymentOrdersReportModel")]
    public class PaymentOrdersReportModel
    {
        public int Id { get; set; }
        public string PaymentOrderNumber { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int PaymentBranchId { get; set; }
        public string PaymentBranchName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime PaymentDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string BeneficiaryDocNumber { get; set; }
        public string BeneficiaryName { get; set; }
        public string PayToName { get; set; }

        //Movement Sumary
        public string DescriptionMovementSumary { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}