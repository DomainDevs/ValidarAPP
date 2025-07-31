using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Nombre de la Regla
    /// </summary>
    [DataContract]
    public class RuleName
    {
        /// <summary>
        /// Identificador de la Regla
        /// </summary>
        [DataMember]
        public int RuleNameId { get; set; }

        /// <summary>
        /// Nombre de la Regla
        /// </summary>
        [DataMember]
        public string Name { get; set; }

    }
}
