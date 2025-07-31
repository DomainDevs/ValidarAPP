using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Claulas de la poliza o temporal
    /// </summary>
    [DataContract]
    public class PolicyClause : Clause
    {
        /// <summary>
        /// Atributo para la propiedad EndorsementId
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }
        
        /// <summary>
        /// Obtener o Asignar el Estado de la clausula
        /// </summary>
        /// <value>
        /// Estado Clausula
        /// </value>
        [DataMember]
        public int ClauseStatus { get; set; }
        /// <summary>
        /// Obtener o Asignar el estado Original de la clausula
        /// </summary>
        /// <value>
        ///  estado de la clausula
        /// </value>
        [DataMember]
        public int OriginalClauseStatus { get; set; }

        /// <summary>
        /// Obtener o Asignar si es la clausula activa
        /// </summary>
        /// <value>
        /// Clausula activa
        /// </value>
        [DataMember]
        public bool IsCurrent { get; set; }
    }
}
