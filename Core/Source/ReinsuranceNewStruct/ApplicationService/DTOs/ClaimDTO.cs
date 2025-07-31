using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
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
        public BranchDTO Branch { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }        
        
        [DataMember]
        public CatastrophicEventDTO CatastrophicEvent { get; set; }
        [DataMember]
        public InspectionDTO Inspection { get; set; }
        [DataMember]
        public CityDTO City { get; set; }
        [DataMember]
        public CauseDTO Cause { get; set; }
        [DataMember]
        public List<ClaimModifyDTO> Modifications { get; set; }
        [DataMember]
        public DamageTypeDTO DamageType { get; set; }
        [DataMember]
        public DamageResponsabilityDTO DamageResponsability { get; set; }

        [DataMember]
        public int CoveredRiskType { get; set; }

        [DataMember]
        public TextOperationDTO TextOperation { get; set; }

        [DataMember]
        public int ClaimCode { get; set; }

        [DataMember]
        public int PrefixCode { get; set; }

        [DataMember]
        public int ClaimBranchCode { get; set; }

        [DataMember]
        public int ClaimNumber { get; set; }

        [DataMember]
        public string ClaimDate { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public decimal EstimationAmount { get; set; }

        [DataMember]
        public decimal PolicyNumber { get; set; }
      
        [DataMember]
        public ClaimEndorsementDTO Endorsement { get; set; }
       
    }
}
