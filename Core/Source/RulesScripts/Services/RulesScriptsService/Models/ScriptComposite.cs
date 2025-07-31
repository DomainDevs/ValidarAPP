using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class ScriptComposite
    {
        /// <summary>
        /// Guon
        /// </summary>
        [DataMember]
        public Script Script { get; set; }

        /// <summary>
        /// Nodos
        /// </summary>
        [DataMember]
        public List<Node> Nodes { get; set; }
    }
}
