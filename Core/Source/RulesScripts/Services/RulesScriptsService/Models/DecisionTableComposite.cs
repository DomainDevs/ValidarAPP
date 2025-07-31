using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class DecisionTableComposite
    {
        /// <summary>
        /// Datos Basicos de la Tabla de Decision 
        /// </summary>
        [DataMember]
        public RuleBase RuleBase {get; set;}

        /// <summary>
        /// Listado De Reglas
        /// </summary>
        [DataMember]
        public List<RuleComposite> RulesComposite { get; set; }
    }
}
