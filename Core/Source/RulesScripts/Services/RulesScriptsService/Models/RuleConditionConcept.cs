using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RuleConditionListEntity
    {
        /// <summary>
        /// Identificador de RuleBase
        /// </summary>
        [DataMember]
        public int RuleBaseId { get; set; }

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
        /// Numero de order de la regla
        /// </summary>
        [DataMember]
        public int OrderNumber { get; set; }
    }
}
