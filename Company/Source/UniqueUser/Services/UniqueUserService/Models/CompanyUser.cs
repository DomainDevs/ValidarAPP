using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniqueUserServices.Models
{
    [DataContract]
    public class CompanyUser : BaseUser
    {
        /// <summary>
        /// Get or set Profile
        /// </summary>
        [DataMember]
        public List<CompanyProfile> Profiles { get; set; }

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
        public List<CompanyBranch> Branch { get; set; }


        /// <summary>
        /// Get or Set IndividualsRelation
        /// </summary>
        [DataMember]
        public List<IndividualRelationApp> IndividualsRelation { get; set; }
        
        /// <summary>
        /// Get or Set Prefix
        /// </summary>
        [DataMember]
        public List<Prefix> Prefixes { get; set; }
		
		/// <summary>
        /// Get or Set UniqueUsersProduct
        /// </summary>
        [DataMember]
        public List<UniqueUsersProduct> UniqueUsersProduct { get; set; }
    }
}
