using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class PayerComponent : BasePayerComponent
    {
        /// <summary>
        /// Componente
        /// </summary>
        [DataMember]
        public Component Component { get; set; }

        /// <summary>
        /// Obtiene o establece las propiedades Dinamicas
        /// </summary>
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        /// <summary>
        /// Cobertura
        /// </summary>
        [DataMember]
        public Coverage Coverage { get; set; }
    }
}
