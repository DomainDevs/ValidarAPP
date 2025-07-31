using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    [DataContract]
    public class GroupInfringement
    {
        [DataMember]
        public int GroupInfringementCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool SnActive { get; set; }

        [DataMember]
        public int InfringementOneYear { get; set; }

        [DataMember]
        public int InfringementThreeYears { get; set; }
    }
}