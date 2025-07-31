using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.DTOs
{
    /// <summary>
    /// Compnentes asociados al impuesto
    /// </summary>
    [DataContract]
    public class TaxPayerDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// Identificador del Impuesto
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ComponentId identifier.
        /// </summary>
        /// <value>
        /// Identificador del Componente
        /// </value>
        [DataMember]
        public int ComponentId { get; set; }

        /// <summary>
        /// Obtiene o setea la Tasa
        /// </summary>
        /// <value>
        ///  la Tasa
        /// </value>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Obtiene o setea la Condicion impuesto
        /// </summary>
        /// <value>
        /// Identificador Condicion
        /// </value>
        [DataMember]
        public int TaxConditionId { get; set; }

        /// <summary>
        /// Gets or sets the current from.
        /// </summary>
        /// <value>
        /// The current from.
        /// </value>
        [DataMember]
        public DateTime CurrentFrom { get; set; }
    }
}
