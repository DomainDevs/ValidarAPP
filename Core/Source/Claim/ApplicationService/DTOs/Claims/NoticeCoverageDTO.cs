using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class NoticeCoverageDTO
    {
        [DataMember]
        public int RiskNum { get; set; }

        [DataMember]
        public int CoverNum { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public bool IsInsured { get; set; }

        [DataMember]
        public bool IsProspect { get; set; }

        [DataMember]
        public int EstimateTypeId { get; set; }

        [DataMember]
        public decimal EstimateAmount { get; set; }

        [DataMember]
        public string CoverageName { get; set; }

        [DataMember]
        public decimal InsurableAmount { get; set; }
        
        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }
    }
}
