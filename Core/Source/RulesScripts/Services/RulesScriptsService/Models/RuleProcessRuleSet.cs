using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Reglas asociadas a los procesos
    /// </summary>
    [DataContract]
    public class RuleProcessRuleSet
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador Process
        /// </summary>
        [DataMember]
        public int ProcessId { get; set; }
        /// <summary>
        /// Regla post
        /// </summary>
        [DataMember]
        public int PosRuleSet { get; set; }

        /// <summary>
        /// Regla pre
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Regla pre
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

    }
}
