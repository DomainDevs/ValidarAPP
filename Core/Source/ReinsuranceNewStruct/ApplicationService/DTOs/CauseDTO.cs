using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    public class CauseDTO
    {
        [DataMember]
        public bool PoliceComplaintRequired { get; set; }
        [DataMember]
        public bool DriverInformationRequired { get; set; }
        [DataMember]
        public bool InspectionDateRequired { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }

    }
}