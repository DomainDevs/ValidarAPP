using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models
{
    /// <summary>
    /// UniqueUserLogin
    /// </summary>
    [DataContract]
    public class UniqueUserLogin : Extension
    {
        /// <summary>
        /// Gets or sets UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password expiration date.
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets ExpirationsDays
        /// </summary>
        [DataMember]
        public int ExpirationsDays { get; set; }

        /// <summary>
        /// Gets or sets MustChangePassword.
        /// </summary>
        [DataMember]
        public bool? MustChangePassword { get; set; }
    }
}
