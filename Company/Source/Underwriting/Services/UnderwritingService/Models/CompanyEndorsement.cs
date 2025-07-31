using System;
using System.Runtime.Serialization;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;

namespace Sistran.Company.Application.UnderwritingServices
{
    [DataContract]
    public class CompanyEndorsement : BaseEndorsement
    {
        [DataMember]
        public CompanyText Text { get; set; }

        [DataMember]
        public string DescriptionRisk { get; set; }

        /// <summary>
        /// SubCoveredRiskType
        /// </summary>       
        [DataMember]
        public int SubCoveredRiskType { get; set; }

        /// <summary>
        /// tipo de endoso para nota credito
        /// </summary>       
        [DataMember]
        public int? CreditNoteEndorsementType { get; set; }

        /// <summary>
        /// id del riesgo para endosos de transportes
        /// </summary>       
        [DataMember]
        public int? RiskId { get; set; }
        /// <summary>
        /// id cobertura  para  endoso  nota credito
        /// </summary>       
        [DataMember]
        public int? CoverageId { get; set; }
        /// <summary>
        /// id objeto del seguro   para endosos de transportes
        /// </summary>       
        [DataMember]
        public int? InsuredObjectId { get; set; }
        /// <summary>
        /// Numero de control
        /// </summary>
        [DataMember]
        public int ControlNumber { get; set; }


        /// <summary>
        /// prima a devolver endoso nota credito
        /// </summary>       
        [DataMember]
        public decimal? PremiumToReturn { get; set; }

        /// <summary>
        /// Valor Declarado  endoso  declaracion
        /// </summary>       
        [DataMember]
        public decimal? DeclaredValue { get; set; }

        /// <summary>
        /// Numero de Radicación
        /// </summary>
        [DataMember]
        public int? TicketNumber { get; set; }

        /// <summary>
        /// Fecha Radicación
        /// </summary>
        [DataMember]
        public DateTime? TicketDate { get; set; }


        /// <summary>
        /// PrevPolicyId
        /// </summary>       
        [DataMember]
        public int? PrevPolicyId { get; set; }

        /// <summary>
        /// Fecha de emisión endoso
        /// </summary>
        [DataMember]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Nro. de Poliza CoAseguro
        /// </summary>
        [DataMember]
        public int BusinessTypeDescription { get; set; }

        /// <summary>
        /// Referencia a R1 o R2
        /// </summary>
        [DataMember]
        public int? AppRelation { get; set; }

        /// <summary>
        /// Marca para colectiva.
        /// </summary>
        [DataMember]
        public bool? isCollective { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o sete el tipo de modificacion
        /// </summary>
        /// <value>
        /// tipo de modificacion
        /// </value>
        [DataMember]
        public int ModificationTypeId { get; set; }

        [DataMember]
        public bool OnlyCancelation { get; set; }
    }
}
