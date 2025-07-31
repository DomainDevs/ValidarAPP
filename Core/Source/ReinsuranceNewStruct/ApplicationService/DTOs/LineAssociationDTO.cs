using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Asociación de Línea
    /// </summary>
    [DataContract]
    public class LineAssociationDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int LineAssociationId { get; set; }

        /// <summary>
        /// Tipo de Asociación
        /// </summary>
        [DataMember]
        public LineAssociationTypeDTO AssociationType { get; set; }

        /// <summary>
        /// Línea
        /// </summary>
        [DataMember]
        public LineDTO Line { get; set; }

        /// <summary>
        /// Fecha Desde
        /// </summary>
        [DataMember]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Fecha Hasta
        /// </summary>
        [DataMember]
        public DateTime DateTo { get; set; }
    }
}