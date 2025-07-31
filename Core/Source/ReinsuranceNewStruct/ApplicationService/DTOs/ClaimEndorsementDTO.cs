using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ClaimEndorsementDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public decimal PolicyNumber { get; set; }

        [DataMember]
        public int RiskId { get; set; }
    }
}
