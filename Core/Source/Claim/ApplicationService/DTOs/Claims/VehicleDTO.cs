using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class VehicleDTO
    {
        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string Chasis { get; set; }

        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public int? MakeId { get; set; }

        [DataMember]
        public string Motor { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int? ColorId { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public int? ModelId { get; set; }

        [DataMember]
        public decimal InsuredAmount { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int NumberBeneficiarie { get; set; }

        [DataMember]
        public string NameBeneficiarie { get; set; }

        [DataMember]
        public decimal ParticipationBeneficiarie { get; set; }

        [DataMember]
        public string InsuredDocumentNum { get; set; }

        [DataMember]
        public string InsuredName { get; set; }

        [DataMember]
        public string InsuredSurName { get; set; }

        [DataMember]
        public string InsuredSecondSurName { get; set; }

        [DataMember]
        public int ProfileId { get; set; }

        [DataMember]
        public string ProfileDescription { get; set; }

        [DataMember]
        public int? VersionId { get; set; }

        [DataMember]
        public string DriverName { get; set; }

        [DataMember]
        public decimal? DocumentNumber { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public int? CoveredRiskType { get; set; }

        [DataMember]
        public int InsuredId { get; set; }
    }
}
