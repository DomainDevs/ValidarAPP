using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RuleComposite
    {
        /// <summary>
        /// Idetificador de la Regla
        /// </summary>
        [DataMember]
        public int RuleId { get; set; }

        /// <summary>
        /// Nombre de la Regla
        /// </summary>
        [DataMember]
        public string RuleName { get; set; }

        /// <summary>
        /// Nombre de la Regla
        /// </summary>
        [DataMember]
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Condiciones
        /// </summary>
        [DataMember]
        public List<Condition> Conditions { get; set; }
        
        /// <summary>
        /// Acciones
        /// </summary>        
        [DataMember]
        public List<Action> Actions { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [DataMember]
        public bool IsTable { get; set; }
    }
}
