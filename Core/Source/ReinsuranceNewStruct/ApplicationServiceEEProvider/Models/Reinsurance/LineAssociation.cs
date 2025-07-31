using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Asociación de Línea
    /// </summary>
    [DataContract]
    public class LineAssociation
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
        public LineAssociationType AssociationType { get; set; }

        /// <summary>
        /// Línea
        /// </summary>
        [DataMember]
        public Line Line { get; set; }

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
