using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ThirdAffected
    {
        [DataMember]
        public int ClaimCoverageId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string FullName { get; set; }
    }
}
