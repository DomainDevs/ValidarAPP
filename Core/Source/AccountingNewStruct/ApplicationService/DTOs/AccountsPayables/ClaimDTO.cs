using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    [DataContract]
    public class ClaimDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public DateTime OccurrenceDate { get; set; }
        [DataMember]
        public int BusinessTypeId { get; set; }
        [DataMember]
        public int? NoticeId { get; set; }
        [DataMember]
        public DateTime NoticeDate { get; set; }
        [DataMember]
        public string RiskDescription { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public int TemporalId { get; set; }
        [DataMember]
        public bool IsTotalParticipation { get; set; }
        [DataMember]
        public int CoveredRiskType { get; set; }

    }
}