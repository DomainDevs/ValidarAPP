using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Edge
    {
        /// <summary>
        /// Id de la Respuesta
        /// </summary>
        [DataMember]
        public int EdgeId { get; set; }

        /// <summary>
        /// Id del Nodo
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }

        /// <summary>
        /// Id de la Pregunta
        /// </summary>
        [DataMember]
        public int QuestionId { get; set; }

        /// <summary>
        /// Id del Guion 
        /// </summary>
        [DataMember]
        public int ScriptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IsDefault { get; set; }

        /// <summary>
        /// Id del Siguiente Nodo
        /// </summary>
        [DataMember]
        public int? NextNodeId { get; set; }
        
        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Valor del Codigo
        /// </summary>
        [DataMember]
        public int ValueCode { get; set; }

        /// <summary>
        /// Lista de Nodos
        /// </summary>
        [DataMember]
        public List<Node> Nodes { get; set; }   
    }
}
