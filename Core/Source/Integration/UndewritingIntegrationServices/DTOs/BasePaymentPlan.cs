
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class BasePaymentPlan
    {
        [DataMember]
        public SummaryDTO SummaryDTO { get; set; }

        [DataMember]
        public List<FinancialPlanDTO> FinancialPlanDTO { get; set; }

    }
}