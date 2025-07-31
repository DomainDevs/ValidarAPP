using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class NoticeCoverage
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int CoverageNumber { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public bool IsInsured { get; set; }

        [DataMember]
        public int EstimateTypeId { get; set; }

        [DataMember]
        public decimal EstimateAmount { get; set; }

        [DataMember]
        public bool IsProspect { get; set; }

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
