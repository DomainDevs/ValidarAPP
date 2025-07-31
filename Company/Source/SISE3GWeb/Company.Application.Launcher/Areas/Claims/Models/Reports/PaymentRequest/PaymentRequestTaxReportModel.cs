using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest
{
    public class PaymentRequestTaxReportModel
    {
        public string TaxCode { get; set; }
        
        public string TaxCategory { get; set; }
        
        public string TaxDescription { get; set; }
        
        public string TaxBaseAmount { get; set; }
        
        public string TaxValue { get; set; }
    }
}