using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class LineCumulusType
    {
        [DataMember]
        public int LineId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int CumulusTypeId { get; set; }

        [DataMember]
        public string CumulusTypeDescription { get; set; }
    }
}
