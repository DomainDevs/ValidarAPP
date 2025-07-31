using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class PoliciesAut : BasePoliciesAut
    {

        [DataMember]
        public GroupPolicies GroupPolicies { set; get; }

        [DataMember]
        public Enums.TypePolicies Type { set; get; }

        [DataMember]
        public List<ConceptDescription> ConceptsDescription { set; get; }

        [DataMember]
        public List<UserAuthorization> UserAuthorization { set; get; }

        [DataMember]
        public List<UserNotification> UserNotification { set; get; }

        [DataMember]
        public _RuleSet RuleSet { set; get; }
    }
}