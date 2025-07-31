using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Coberturas
    /// </summary>
    [DataContract]
    public class Coverage : BaseCoverage
    {
        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// Textos
        /// </summary>
        [DataMember]
        public Text Text { get; set; }

        /// <summary>
        /// Lista de clausulas
        /// </summary>
        [DataMember]
        public List<Clause> Clauses { get; set; }

        /// <summary>
        /// Atributo para la propiedad Deductible
        /// </summary> 
        [DataMember]
        public Deductible Deductible { get; set; }

        /// <summary>
        /// Obtiene o establece las propiedades Dinamicas
        /// </summary>
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        /// <summary>
        /// objeto del seguro
        /// </summary>
        [DataMember]
        public InsuredObject InsuredObject { get; set; }

        /// <summary>
        /// Gets or sets las coberturas aliadas
        /// </summary>        
        [DataMember]
        public List<Coverage> CoverageAllied { get; set; }

        /// <summary>
        /// Gets or sets las coberturas aliadas
        /// </summary>        
        [DataMember]
        public int? AllyCoverageId { get; set; }
        /// <summary>

    }
}
