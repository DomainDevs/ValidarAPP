using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.BaseEndorsementService.DTOs
{
    /// <summary>
    /// Tipo de modificacion
    /// </summary>
    [DataContract]
    public class EndorsementTypeDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Int16 Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}
