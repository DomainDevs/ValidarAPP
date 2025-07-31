using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.SecurityServices.Models.Base
{
    [DataContract]
    public class BaseProfile : Extension
    {
        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        /// <value>
        /// The profile identifier.
        /// </value>
        [DataMember]
        public int ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expiration date null.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expiration date null; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsExpirationDateNull { get; set; }
    }
}
