using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Concepto
    /// </summary>
    [DataContract]
    public class RangeConcept: Concept
    {
        /// <summary>
        /// Codigo del Rango de la Entidad
        /// </summary>
        [DataMember]
        public int RangeEntityCode { get; set; }

        /// <summary>
        /// Valor de la Entidad
        /// </summary>
        [DataMember]
        public List<RangeEntityValue> EntityValues { get; set; }
    }
}
