using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest
{
    public class PaymentRequestAccountingReportModel
    {
        public string Account { get; set; }

        public string Description { get; set; }

        public string DebitAmount { get; set; }

        public string CreditAmount { get; set; }
    }
}