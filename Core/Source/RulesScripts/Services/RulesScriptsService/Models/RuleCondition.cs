using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RuleCondition
    {
        /// <summary>
        /// Identificador de RuleBase
        /// </summary>
        [DataMember]
        public int RuleBaseId { get; set; }

        /// <summary>
        /// Identificador de Regla
        /// </summary>
        [DataMember]
        public int RuleId { get; set; }

        /// <summary>
        /// Identificador de Condición
        /// </summary>
        [DataMember]
        public int ConditionId { get; set; }

        /// <summary>
        /// Identificador de Concepto
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Identificador de Entidad
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Código Comparador
        /// </summary>
        [DataMember]
        public int? ComparatorCode { get; set; }

        /// <summary>
        /// Tipo de Valor de regla
        /// </summary>
        [DataMember]
        public int RuleValueTypeCode { get; set; }     
        
        /// <summary>
        /// Valor de codición
        /// </summary>
        [DataMember]
        public int ConditionValue { get; set; }

        [DataMember]
        public string CondValue { get; set; }
        
        /// <summary>
        /// Numero de order de la regla
        /// </summary>
        [DataMember]
        public int OrderNumber { get; set; }
    }
}
