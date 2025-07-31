using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{

    /// <summary>
    /// Temporal
    /// </summary>
    [DataContract]
    public class TemporalDTO
    {
        /// <summary>
        /// Obtiene o establece Id del endos0
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de endoso
        /// </summary>
        /// <value>
        /// The type of the endorsement.
        /// </value>
        [DataMember]
        public int EndorsementType { get; set; }

        /// <summary>
        /// Gets or sets the endorsement identifier.
        /// </summary>
        /// <value>
        /// The endorsement identifier.
        /// </value>
        [DataMember]
        public int EndorsementId { get; set; }

    }
}
