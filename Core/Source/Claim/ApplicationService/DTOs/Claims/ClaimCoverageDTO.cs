using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimCoverageDTO
    {
        /// <summary>
        /// Identificador del subsiniestro
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Número del subsiniestro
        /// </summary>
        [DataMember]
        public int SubClaim { get; set; }

        /// <summary>
        /// Identificador de la modificación
        /// </summary>
        [DataMember]
        public int ClaimModifyId { get; set; }

        /// <summary>
        /// Identificador del riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Número del riesgo
        /// </summary>
        [DataMember]
        public int RiskNum { get; set; }

        /// <summary>
        /// Identificador de la cobertura
        /// </summary>
        [DataMember]
        public int CoverageId { get; set; }

        /// <summary>
        /// Número de la cobertura
        /// </summary>
        [DataMember]
        public int CoverageNum { get; set; }

        /// <summary>
        /// Suma asegurada de la cobertura
        /// </summary>
        [DataMember]
        public decimal CoverageInsuredAmount { get; set; }

        /// <summary>
        /// Identificador del individuo
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Es asegurado?
        /// </summary>
        [DataMember]
        public bool IsInsured { get; set; }

        /// <summary>
        /// Identificador del endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Es tercero?
        /// </summary>
        [DataMember]
        public bool IsProspect { get; set; }

        /// <summary>
        /// Suma asegurada
        /// </summary>
        [DataMember]
        public decimal? InsuredAmountTotal { get; set; }

        /// <summary>
        /// Descripción del subsiniestro
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Estimaciones del subsiniestro
        /// </summary>
        [DataMember]
        public List<EstimationDTO> Estimations { get; set; }

        /// <summary>
        /// Bienes afectados
        /// </summary>
        [DataMember]
        public string AffectedProperty { get; set; }

        [DataMember]
        public DriverInformationDTO DriverInformationDTO { get; set; }

        [DataMember]
        public ThirdAffectedDTO ThirdAffectedDTO { get; set; }

        [DataMember]
        public ThirdPartyVehicleDTO ThirdPartyVehicleDTO { get; set; }

        [DataMember]
        public decimal ClaimedAmount { get; set; }

        [DataMember]
        public bool IsClaimedAmount { get; set; }
    }
}
