using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Paquete de Reglas Compuestas
    /// </summary>
    [DataContract]
    public class RuleSetComposite
    {
        /// <summary>
        /// Paquete de Reglas
        /// </summary>
        [DataMember]
        public RuleSet RuleSet { get; set; }

        /// <summary>
        /// Reglas Compuestas
        /// </summary>
        [DataMember]
        public List<RuleComposite> RuleComposites { get; set; }
    }
}
