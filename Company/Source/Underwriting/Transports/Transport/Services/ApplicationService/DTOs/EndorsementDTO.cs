using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Transports.TransportApplicationService.DTOs
{
    [DataContract]
    public class EndorsementDTO
    {
        [DataMember]
        public decimal PolicyNumber { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int IdEndorsement { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember] 
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public bool IsCurrent { get; set; }
        [DataMember]
        public int TemporalId { get; set; } 
        [DataMember]
        public EndorsementType? EndorsementType { get; set; }
        [DataMember]
        public int? RiskId { get; set; }

    }
}