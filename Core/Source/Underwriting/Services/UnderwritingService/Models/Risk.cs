using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Riesgo
    /// </summary>
    [DataContract]
    [Serializable]
    public class Risk : BaseRisk
    {
        /// <summary>
        /// Textos
        /// </summary>
        [DataMember]
        public virtual Text Text { get; set; }

        /// <summary>
        /// Lista de clausulas
        /// </summary>
        [DataMember]
        public virtual List<Clause> Clauses { get; set; }

        /// <summary>
        /// Lista de beneficiarios
        /// </summary>
        [DataMember]
        public virtual List<Beneficiary> Beneficiaries { get; set; }

        /// <summary>
        /// Grupo de coberturas
        /// </summary>
        [DataMember]
        public virtual GroupCoverage GroupCoverage { get; set; }

        /// <summary>
        /// Lista de coberturas
        /// </summary>
        [DataMember]
        public virtual List<Coverage> Coverages { get; set; }

             /// <summary>
        /// Asegurado principal
        /// </summary>
        [DataMember]
        public IssuanceInsured MainInsured { get; set; }

        /// <summary>
        /// Listado de asegurados
        /// </summary>
        [DataMember]
        public IssuanceInsured SecondInsured { get; set; }

        /// <summary>
        /// Zona de Tarifacion
        /// </summary>
        [DataMember]
        public virtual RatingZone RatingZone { get; set; }

        /// <summary>
        /// Obtiene o establece las propiedades Dinamicas
        /// </summary>
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        
        /// Limite RC
        /// </summary>
        [DataMember]
        public LimitRc LimitRc { get; set; }

        /// <summary>
        /// Póliza
        /// </summary>
        [DataMember]
        public Policy Policy { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        /// <summary>
        ///Actividad del riesgo
        /// </summary>
        [DataMember]
        public RiskActivity RiskActivity { get; set; }

    }
}