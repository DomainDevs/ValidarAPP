using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Audit
{
    /// <summary>
    /// Paquetes
    /// </summary>
    [DataContract]
    public class PackageServiceModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public String Description { get; set; }
    }
}
