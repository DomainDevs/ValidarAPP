using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ProductServices.DTOs
{

    /// <summary>
    /// Tipoi de Riesgo
    /// </summary>
    [DataContract]
    public class SubCoverageDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Int16 Id { get; set; }
    }
}
