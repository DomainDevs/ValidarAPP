using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class UserAuthorization : BaseUserAuthorization
    {
        [DataMember]
        public PoliciesAut Policies { set; get; }

        [DataMember]
        public User User { set; get; }

        [DataMember]
        public CoHierarchyAssociation Hierarchy { set; get; }
    }
}
