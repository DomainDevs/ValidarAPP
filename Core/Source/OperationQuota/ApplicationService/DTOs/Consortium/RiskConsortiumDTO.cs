using System.Runtime.Serialization;

namespace Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium
{
    [DataContract]
    public class RiskConsortiumDTO
    {
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public int ConsortiumId { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public decimal PjePart { get; set; }
    }
}
