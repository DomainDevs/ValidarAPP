using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest
{
    [KnownType("PaymentRequestReportModel")]
    public class PaymentRequestReportModel
    {
        public string Number { get; set; }
        
        public string ReportDate { get; set; }
        
        public string Branch { get; set; }
        
        public string ContractCity { get; set; }
        
        public string PolicyAgent { get; set; }
        
        public string Prefix { get; set; }
        
        public string PolicyNumber { get; set; }
        
        public string ClaimNumber { get; set; }
        
        public string ClaimRegistrationDate { get; set; }
        
        public string PolicyHolder { get; set; }
        
        public string PolicyInsured { get; set; }

        public string PaymentBeneficiaryPersonType { get; set; }

        public string PaymentBeneficiaryName { get; set; }
        
        public string PaymentBeneficiaryDocumentNumber { get; set; }
        
        public string PaymentTechnicalTransaction { get; set; }
        
        public string VoucherType { get; set; }
        
        public string PaymentMethod { get; set; }
        
        public string PaymentCurrency { get; set; }
        
        public string VoucherCurrency { get; set; }
        
        public string CostCenter { get; set; }
        
        public string PaymentTotalAmount { get; set; }
        
        public string TRM { get; set; }
        
        public string TotalAmountConcepts { get; set; }
        
        public string PaymentDescription { get; set; }
    }
}