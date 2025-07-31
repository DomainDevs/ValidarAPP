using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Question
    {
        /// <summary>
        /// Id de la Pregunta
        /// </summary>
        [DataMember]
        public int QuestionId { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id del Concepto
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Id de la Entidad
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Orden la Pregunta
        /// </summary>
        [DataMember]
        public int OrdenNum { get; set; }

        /// <summary>
        /// Listado de Respuestas
        /// </summary>
        [DataMember]
        public List<Edge> Edges { get; set; }
    }
}
