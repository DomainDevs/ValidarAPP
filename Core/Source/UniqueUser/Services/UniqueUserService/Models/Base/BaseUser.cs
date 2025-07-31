using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseUser : Extension
    {
        /// <summary>
        /// Get or set UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Get or set CreatedUserId
        /// </summary>
        [DataMember]
        public int CreatedUserId { get; set; }

        /// <summary>
        /// Get or set ModifiedUserId
        /// </summary>
        [DataMember]
        public int ModifiedUserId { get; set; }

        /// <summary>
        /// Get or set PersonId
        /// </summary>
        [DataMember]
        public int PersonId { get; set; }

        /// <summary>
        /// Get or set  AccountName
        /// </summary>
        [DataMember]
        public string AccountName { get; set; }

        /// <summary>
        /// Get or set Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Get or set CreationDate
        /// </summary>
        [DataMember]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Get or set LastModificationDate
        /// </summary>
        [DataMember]
        public DateTime? LastModificationDate { get; set; }


        /// <summary>
        /// Get or set ExpirationDate
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Get or set LockDate
        /// </summary>
        [DataMember]
        public DateTime? LockDate { get; set; }

        /// <summary>
        /// Get or set DisableDate
        /// </summary>
        [DataMember]
        public DateTime? DisableDate { get; set; }

        /// <summary>
        /// Get or set UserDomain
        /// </summary>
        [DataMember]
        public string UserDomain { get; set; }

        [DataMember]
        public bool IsDisabledDateNull { get; set; }

        [DataMember]
        public bool IsExpirationDateNull { get; set; }

        [DataMember]
        public bool IsLockDateNull { get; set; }

        [DataMember]
        public bool LockPassword { get; set; }

        [DataMember]
        public int AuthenticationTypeCode { get; set; }

        [DataMember]
        public string StatusDescription { get; set; }
		
        /// <summary>
        /// Get or set AuthenticationType
        /// </summary>
        [DataMember]
        public Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType AuthenticationType { get; set; }
    }
}
