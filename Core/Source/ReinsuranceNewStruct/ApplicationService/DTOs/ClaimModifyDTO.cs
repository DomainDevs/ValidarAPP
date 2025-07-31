using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ClaimModifyDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime AccountingDate { get; set; }
        [DataMember]
        public DateTime RegistrationDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int SubClaim { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public int RiskNumber { get; set; }
        [DataMember]
        public string RiskDescription { get; set; }
        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public int CoverageNumber { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public bool IsInsured { get; set; }
        [DataMember]
        public bool IsProspect { get; set; }
        [DataMember]
        public decimal? SubLimitAmount { get; set; }
        [DataMember]
        public List<ClaimCoverageDTO> Coverages { get; set; }
    }
}