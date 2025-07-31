using System.Collections.Generic;

namespace Sistran.Core.Application.UnderwritingServices.Models.Distribution
{
    public class PaymentDistributionPlan
    {
        public int Number { get; set; }
        public List<PaymentDistributionComp> PaymentDistributionComp { get; set; }

    }
}
