using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.DeclarationApplicationService.DTO
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
        /// lista de riesgos
        /// </summary>
        [DataMember]
        public List<CompanyPropertyRisk> Risks { get; set; }
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
        /// objeto de riesgo
        /// </summary>
        [DataMember]
        public CompanyPropertyRisk Risk { get; set; }
        /// <summary>
        /// Objeto de endoso
        /// </summary>
        [DataMember]
        public EndorsementDTO Endorsment { get; set; }
        /// <summary>
        /// objeto de cobertura
        /// </summary>
        [DataMember]
        public CoverageDTO coverage { get; set; }
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
		
        [DataMember]
        public int TicketNumber { get;set;}
		
        [DataMember]
        public DateTime TicketDate { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Fecha finalización de la vigencia
        /// </summary>
        //[DataMember]
        //public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Fecha de inicio de la vigencia
        /// </summary>
        //[DataMember]
        //public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Cantidad de dias para la vigencia
        /// </summary>
        //[DataMember]
        //public int Days { get; set; }

        /// <summary>
        /// Indica si es el endoso actual de la póliza
        /// </summary>
        //[DataMember]
        //public bool Current { get; set; }

    }
}
