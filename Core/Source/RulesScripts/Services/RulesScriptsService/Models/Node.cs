using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Node
    {
        /// <summary>
        /// Id del Nodo
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }

        /// <summary>
        /// Id del Guion
        /// </summary>
        [DataMember]
        public int ScriptId { get; set; }

        /// <summary>
        /// Listado de Preguntas
        /// </summary>
        [DataMember]
        public List<Question> Questions { get; set; }
    }
}
