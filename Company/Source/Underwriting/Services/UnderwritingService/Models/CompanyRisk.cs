using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Riesgo
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UnderwritingServices.Models.Risk" />
    [DataContract]
    public class CompanyRisk : BaseRisk
    {
        /// <summary>
        /// Textos
        /// </summary>
        [DataMember]
        public virtual CompanyText Text { get; set; }

        /// <summary>
        /// Gets or sets the main insured.
        /// </summary>
        /// <value>
        /// The main insured.
        /// </value>
        [DataMember]
        public CompanyIssuanceInsured MainInsured { get; set; }

        /// <summary>
        /// Gets or sets the policy.
        /// </summary>
        /// <value>
        /// The policy.
        /// </value>
        [DataMember]
        public CompanyPolicy Policy { get; set; }

        /// <summary>
        /// Gets or sets the second insured.
        /// </summary>
        /// <value>
        /// The second insured.
        /// </value>
        [DataMember]
        public CompanyIssuanceInsured SecondInsured { get; set; }

        /// <summary>
        /// Lista de coberturas company
        /// </summary>
        /// <value>
        /// The coverages.
        /// </value>
        [DataMember]
        public List<CompanyCoverage> Coverages { get; set; }

        /// <summary>
        /// Información de siniestralidad
        /// </summary>
        /// <value>
        /// The company claims bills.
        /// </value>
        [DataMember]
        public CompanyClaimsBills CompanyClaimsBills { get; set; }

        /// <summary>
        /// Lista de beneficiarios
        /// </summary>
        /// <value>
        /// The beneficiaries.
        /// </value>
        [DataMember]
        public virtual List<CompanyBeneficiary> Beneficiaries { get; set; }

        /// <summary>
        /// Grupo de coberturas
        /// </summary>
        /// <value>
        /// The group coverage.
        /// </value>
        [DataMember]
        public virtual GroupCoverage GroupCoverage { get; set; }

        /// <summary>
        /// Limite RC
        /// </summary>
        /// <value>
        /// The limit rc.
        /// </value>
        [DataMember]
        public CompanyLimitRc LimitRc { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        /// <summary>
        ///Actividad del riesgo
        /// </summary>
        [DataMember]
        public CompanyRiskActivity RiskActivity { get; set; }


        /// <summary>
        /// Obtiene o establece las propiedades Dinamicas
        /// </summary>
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        /// <summary>
        /// Lista de clausulas
        /// </summary>
        [DataMember]
        public virtual List<CompanyClause> Clauses { get; set; }

        /// <summary>
        /// Zona de Tarifacion
        /// </summary>
        [DataMember]
        public virtual CompanyRatingZone RatingZone { get; set; }

        /// <summary>
        /// Cantidad de trailers
        /// </summary>
        [DataMember]
        public int? TrailersQuantity { get; set; }

        /// <summary>
        /// Tipo de Carga
        /// </summary>
        [DataMember]
        public int? LoadTypeId { get; set; }

        /// <summary>
        /// Tipo de Trabajador
        /// </summary>
        [DataMember]
        public int? WorkerType { get; set; }

        /// <summary>
        /// Ispeccion
        /// </summary>
        [DataMember]
        public int? Inspection { get; set; }


        /// <summary>
        /// HasSinister
        /// </summary>
        [DataMember]

        public bool HasSinister { get; set; }


        /// <summary>
        /// AssistanceType
        /// </summary>
        [DataMember]
        public CompanyAssistanceType AssistanceType { get; set; }


        /// <summary>
        /// ActualDateMovement
        /// </summary>
        [DataMember]
        public DateTime ActualDateMovement { get; set; }


        /// <summary>
        /// IssueDate
        /// </summary>
        [DataMember]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Is Facultative
        /// </summary>
        [DataMember]
        public bool? IsFacultative { get; set; }

        /// <summary>
        /// Fecha Actual del riesgo
        /// </summary>
        [DataMember]
        public DateTime? CurrentFrom { get; set; }
    }
}