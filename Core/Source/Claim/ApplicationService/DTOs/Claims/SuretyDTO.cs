using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class SuretyDTO
    {

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string SuretyName { get; set; }

        [DataMember]
        public string SuretyDocumentNumber { get; set; }

        [DataMember]
        public string IdentificationDocument { get; set; }

        [DataMember]
        public string Bonded { get; set; }

        [DataMember]
        public string BidNumber { get; set; }

        [DataMember]
        public string CourtNum { get; set; }

        [DataMember]
        public decimal ContractAmt { get; set; }

        [DataMember]
        public decimal PremiumAmt { get; set; }

        [DataMember]
        public decimal EstimationAmount { get; set; }

        [DataMember]
        public int CoveredRiskType { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public decimal? DocumentNum { get; set; }

        [DataMember]
        public int? Endorsement { get; set; }

        [DataMember]
        public int? InsuredId { get; set; }

        [DataMember]
        public decimal InsuredAmount { get; set; }

        [DataMember]
        public int ArticleId { get; set; }

        [DataMember]
        public string ArticleDescription { get; set; }
    }
}
