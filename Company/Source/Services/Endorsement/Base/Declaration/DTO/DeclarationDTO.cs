using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Declaration.DTO
{
    /// <summary>
    /// Modelo para una declaracion
    /// </summary>
    [DataContract]
    public class DeclarationDTO : PolicyDTO
    {
        /// <summary>
        /// texto
        /// </summary>
        [DataMember]
        public String Text { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public String Observation { get; set; }

        /// <summary>
        /// Valor Declarado
        /// </summary>
        [DataMember]
        public decimal? DeclaredValue { get; set; }

        /// <summary>
        /// id del riesgo
        /// </summary>
        [DataMember]
        public int? RiskId { get; set; }

        /// <summary>
        /// id del grupo de cobertura del riesgo
        /// </summary>
        [DataMember]
        public int groupCoverageId { get; set; }

        /// <summary>
        /// id del objeto del seguro
        /// </summary>
        [DataMember]
        public int? InsuranceObjectId { get; set; }

        /// <summary>
        /// cabecera
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// id de la sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// id del ramo
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// numero de la poliza
        /// </summary>
        [DataMember]
        public decimal PolicyNumber { get; set; }

        /// <summary>
        /// id de la póliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// temporal del endoso de declaración
        /// </summary>
        [DataMember]
        public int? TemporalId { get; set; }
        
        /// <summary>
        /// lista de seguros
        /// </summary>
        [DataMember]
        public List<InsuredObjectDTO> Insured { get; set; }

        /// <summary>
        /// Lista de endosos
        /// </summary>
        [DataMember]
        public List<EndorsementDTO> Endorsments { get; set; }

        /// <summary>
        /// objeto de enums de endosos
        /// </summary>
        [DataMember]
        public int EnumsEndorsment { get; set; }
        
        /// <summary>
        /// Objeto de endoso
        /// </summary>
        [DataMember]
        public EndorsementDTO Endorsment { get; set; }

        /// <summary>
        /// objeto de cobertura
        /// </summary>
        [DataMember]
        public CoverageDTO Coverage { get; set; }

        /// <summary>
        /// Id endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Summary
        /// </summary>
        [DataMember]
        public SummaryDTO Summary { get; set; }

        /// <summary>
        /// Controlador para el enodoso
        /// </summary>
        [DataMember]
        public string EndorsementController { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public int TicketNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public DateTime TicketDate { get; set; }
        
    }
}
