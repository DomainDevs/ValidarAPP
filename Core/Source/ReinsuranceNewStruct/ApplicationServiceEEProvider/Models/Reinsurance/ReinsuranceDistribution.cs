using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsuranceDistribution
    {
        [DataMember]
        public string Line { get; set; }

        [DataMember]
        public string CumulusKey { get; set; }

        [DataMember]
        public string IsFacultative { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string Premium { get; set; }

        [DataMember]
        public string Commission { get; set; }

        [DataMember]
        public string Contract { get; set; }

        [DataMember]
        public int LevelNumber { get; set; }

        [DataMember]
        public string Broker { get; set; }

        [DataMember]
        public string Reinsurer { get; set; }

        [DataMember]
        public int IssueLayerId { get; set; }

        [DataMember]
        public string ParticipationPercentage { get; set; }
    }
}
