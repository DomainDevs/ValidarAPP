using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsuranceMassiveHeader
    {
        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int Records { get; set; }

        [DataMember]
        public string Option { get; set; }

        [DataMember]
        public string DateClose { get; set; }
    }
}
