using Sistran.Core.Integration.UniqueUserServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
    public  class UserDTO
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
        public UniqueUserTypes AuthenticationType { get; set; }
        /// <summary>
        /// Get or set Profile
        /// </summary>
        [DataMember]
        public List<ProfileDTO> Profiles { get; set; }

        /// <summary>
        /// Get or set UniqueUserLogin 
        /// </summary>
        [DataMember]
        public UniqueUserLoginDTO UniqueUsersLogin { get; set; }

        /// <summary>
        /// Get or Set HierachiesAccess
        /// </summary>
        [DataMember]
        public List<CoHierarchyAssociationDTO> Hierarchies { get; set; }

        /// <summary>
        /// Get or Set UserBranch
        /// </summary>
        [DataMember]
        public List<BranchDTO> Branch { get; set; }


        /// <summary>
        /// Get or Set IndividualsRelation
        /// </summary>
        [DataMember]
        public List<IndividualRelationAppDTO> IndividualsRelation { get; set; }

        [DataMember]
        public List<PrefixDTO> Prefixes { get; set; }
    }
}
