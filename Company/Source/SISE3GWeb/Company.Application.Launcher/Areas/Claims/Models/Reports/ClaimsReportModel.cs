using Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports
{
    [KnownType("ClaimsReportModel")]
    public class ClaimsReportModel
    {
        public List<PaymentRequestReportModel> SummaryPaymentRequestReport { get; set; }
    }
}