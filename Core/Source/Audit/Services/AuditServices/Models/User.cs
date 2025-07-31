namespace Sistran.Core.Application.AuditServices.Models
{
    using System.Runtime.Serialization;
    /// <summary>
    /// Usuario
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        ///Identificador del Asuario
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre Usuario
        /// </summary>
        /// <value>
        /// Nombre Usuario
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}
