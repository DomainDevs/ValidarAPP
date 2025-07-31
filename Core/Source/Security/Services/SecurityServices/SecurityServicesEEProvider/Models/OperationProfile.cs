using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.EEProvider.Models
{
    /// <summary>
    /// Perfil Asociado a los Accesos
    /// </summary>
    [DataContract]
    public class OperationProfile
    {
        /// <summary>
        /// Obtiene o setea el Id base de Datos
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        public int DatabaseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OperationProfile"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the operation identifier.
        /// </summary>
        /// <value>
        /// The operation identifier.
        /// </value>
        public int OperationId { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        /// <value>
        /// The profile identifier.
        /// </value>
        public int ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expiration date null.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expiration date null; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpirationDateNull { get; set; }
    }
}
