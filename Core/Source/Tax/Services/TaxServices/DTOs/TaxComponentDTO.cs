using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.DTOs
{
    /// <summary>
    /// Compnentes asociados al impuesto
    /// </summary>
    [DataContract]
        public class TaxComponentDTO
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
    }
}
