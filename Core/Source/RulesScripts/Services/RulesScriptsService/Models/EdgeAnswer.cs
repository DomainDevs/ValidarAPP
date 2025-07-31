using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class EdgeAnswer
    {
        /// <summary>
        /// Id de la Respuesta
        /// </summary>
        [DataMember]
        public int EdgeId { get; set; }

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
        /// Valor del Codigo
        /// </summary>
        [DataMember]
        public int ValueCode { get; set; }
    }
}
