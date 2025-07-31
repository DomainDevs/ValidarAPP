using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class DynamicConcept : ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }
        /// <summary>
        /// Obtiene o establece el identificador del concepto dinamico
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public object Value { get; set; }

        /// <summary>
        /// Nombre completo del tipo de dato del concepto
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// Nombre completo del tipo de dato del concepto
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Nombre completo del tipo de dato del concepto
        /// </summary>
        [DataMember]
        public int? QuestionId { get; set; }

        /// <summary>
        /// Nombre completo del tipo de dato del concepto
        /// </summary>
        [DataMember]
        public string ValueType { get; set; }
    }
}
