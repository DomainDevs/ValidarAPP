using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class TemporalPremiumDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool IsTemporal { get; set; }
        [DataMember]
        public bool moduleId { get; set; }
        [DataMember]
        public bool SourceId { get; set; }
    }
}
