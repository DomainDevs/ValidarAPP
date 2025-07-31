using Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class GroupPolicies : BaseGroupPolicies
    {
        [DataMember]
        public Module Module { set; get; }

        [DataMember]
        public SubModule SubModule { set; get; }

        [DataMember]
        public _Package Package { set; get; }

    }
}
