using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Regla de Tabla de Decisión
    /// </summary>
    [DataContract]
    public class Rule
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
        /// Numero de order de la regla
        /// </summary>
        [DataMember]
        public int OrderNumber { get; set; }
    }
}
