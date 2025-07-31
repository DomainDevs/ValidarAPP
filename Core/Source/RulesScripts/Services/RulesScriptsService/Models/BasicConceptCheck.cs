using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BasicConceptCheck
    {
        /// <summary>
        /// Codigo de Limites de Conceptos Basic
        /// </summary>
        [DataMember]
        public int BasicConceptCode { get; set; }

        /// <summary>
        /// Identificador de Entidad
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Identificador de concepto
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Valor de limites de enteros y decimales
        /// </summary>
        [DataMember]
        public int? IntValue { get; set; }
 
        /// <summary>
        /// Valor de limites de Fechas
        /// </summary>
        [DataMember]
        public DateTime? DateValue { get; set; }
    }
}
