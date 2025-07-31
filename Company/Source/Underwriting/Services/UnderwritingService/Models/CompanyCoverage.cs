using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Coberturas extension
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UnderwritingServices.Models.Coverage" />
    [DataContract]
    public class CompanyCoverage : BaseCoverage
    {
        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public CompanySubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// Textos
        /// </summary>
        [DataMember]
        public CompanyText Text { get; set; }

        /// <summary>
        /// Lista de clausulas
        /// </summary>
        [DataMember]
        public List<CompanyClause> Clauses { get; set; }

        /// <summary>
        /// Atributo para la propiedad Deductible
        /// </summary> 
        [DataMember]
        public CompanyDeductible Deductible { get; set; }

        /// <summary>
        /// Obtiene o establece las propiedades Dinamicas
        /// </summary>
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        /// <summary>
        /// objeto del seguro
        /// </summary>
        [DataMember]
        public CompanyInsuredObject InsuredObject { get; set; }

        /// <summary>
        /// Gets or sets las coberturas aliadas
        /// </summary>        
        [DataMember]
        public List<CompanyCoverage> CoverageAllied { get; set; }

        /// <summary>
        /// Gets or sets IsEnabledMinimumPremium
        /// </summary>        
        [DataMember]
        public bool IsEnabledMinimumPremium { get; set; }
        /// <summary>
        /// Gets or sets las coberturas aliadas
        /// </summary>        
        [DataMember]
        public int? AllyCoverageId { get; set; }


        /// <summary>
        /// Gets or sets IsPostcontractual
        /// </summary>        
        [DataMember]
        public bool IsPostcontractual { get; set; }

    }
}