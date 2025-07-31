using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest
{
    public class PaymentRequestDetailReportModel
    {
        public string ClaimNumber { get; set; }
        
        public string SubClaim { get; set; }
        
        public string BusinessTurn { get; set; }
        
        public string Coverage { get; set; }
        
        public string Deducible { get; set; }
        
        public string Compensation { get; set; }
        
        public string Expenses { get; set; }
        
        public string Reinsurance { get; set; }
    }
}