using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("ReportModel")]
    public class ReportModel
    {
        public List<PaymentDetailsModel> SumaryPaymentReport { get; set; }
        public List<CollectDetailsModel> SumaryCollectReport { get; set; }
        public List<MainReportModel> SumaryMainReport { get; set; }
        public List<BillingCollectDetailsModel> SumaryBillingReport { get; set; }
        public List<BillingCollectDetailsModel> SumaryBillingCollectReport { get; set; }
    }
}