using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class IssGetDistributionErrors
    {
        [DataMember]
        public int TmpReinsuranceProcessId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoverageNumber { get; set; }
    }
}
