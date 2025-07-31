namespace Sistran.Core.Application.AuditServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Paquetes
    /// </summary>
    [DataContract]
    public class Entity
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
        public string Description { get; set; }
    }
}
