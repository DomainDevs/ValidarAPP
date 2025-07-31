using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class TempLineCumulusIssuance
    {
        [DataMember]
        public int TempLayerLineId { get; set; }

        [DataMember]
        public int LineId { get; set; }

        [DataMember]
        public string LineDescription { get; set; }

        [DataMember]
        public string CumulusType { get; set; }

        [DataMember]
        public string CumulusKey { get; set; }

        [DataMember]
        public decimal RetainedSum { get; set; }

        [DataMember]
        public decimal GivenSum { get; set; }
    }
}
