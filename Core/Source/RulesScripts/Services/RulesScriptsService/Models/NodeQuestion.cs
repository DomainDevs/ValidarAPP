using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class NodeQuestion
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
        /// Id de la Pregunta
        /// </summary>
        [DataMember]
        public int QuestionId { get; set; }

        /// <summary>
        /// Orden del la Pregunta
        /// </summary>
        [DataMember]
        public int OrderNum { get; set; }
    }
}
