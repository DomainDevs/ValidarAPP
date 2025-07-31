using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class TransportDTO
    {
        [DataMember]
        public string CityFromDescription { get; set; }

        [DataMember]
        public string CityToDescription { get; set; }

        [DataMember]
        public int CityFromId { get; set; }

        [DataMember]
        public int CityToId { get; set; }

        [DataMember]
        public int? RiskId { get; set; }

        [DataMember]
        public string Risk { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public string CargoTypeDescription { get; set; }

        [DataMember]
        public int CargoTypeId { get; set; }

        [DataMember]
        public string PackagingTypeDescription { get; set; }

        [DataMember]
        public int PackagingTypeId { get; set; }

        [DataMember]
        public string ViaTypeDescription { get; set; }

        [DataMember]
        public int ViaTypeId { get; set; }

        [DataMember]
        public decimal InsuredAmount { get; set; }

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
    }
}
