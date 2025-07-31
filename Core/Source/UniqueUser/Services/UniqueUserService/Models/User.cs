using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    /// <summary>
    /// User
    /// </summary>
    [DataContract]
    public class User : BaseUser
    {
        /// <summary>
        /// Get or set Profile
        /// </summary>
        [DataMember]
        public List<Profile> Profiles { get; set; }

        /// <summary>
        /// Get or set UniqueUserLogin 
        /// </summary>
        [DataMember]
        public UniqueUserLogin UniqueUsersLogin { get; set; }

        /// <summary>
        /// Get or Set HierachiesAccess
        /// </summary>
        [DataMember]
        public List<CoHierarchyAssociation> Hierarchies { get; set; }

        /// <summary>
        /// Get or Set UserBranch
        /// </summary>
        [DataMember]
        public List<Branch> Branch { get; set; }
       

        /// <summary>
        /// Get or Set IndividualsRelation
        /// </summary>
        [DataMember]
        public List<IndividualRelationApp> IndividualsRelation { get; set; }
        
        [DataMember]
        public List<Prefix> Prefixes { get; set; }
    }
}
