using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class AirCraftDTO
    {
        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public int? MakeId { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public int? ModelId { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int? TypeId { get; set; }

        [DataMember]
        public string Use { get; set; }

        [DataMember]
        public int? UseId { get; set; }

        [DataMember]
        public string Register { get; set; }

        [DataMember]
        public int? RegisterId { get; set; }

        [DataMember]
        public string Operator { get; set; }

        [DataMember]
        public int? OperatorId { get; set; }

        [DataMember]
        public int? Year { get; set; }

        [DataMember]
        public string RegisterNumber { get; set; }

        [DataMember]
        public int? RiskId { get; set; }

        [DataMember]
        public string Risk { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoveredRiskType { get; set; }

        [DataMember]
        public decimal PolicyDocumentNumber { get; set; }

        [DataMember]
        public int? PolicyId { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public int InsuredId { get; set; }

        [DataMember]
        public decimal InsuredAmount { get; set; }
    }
}
