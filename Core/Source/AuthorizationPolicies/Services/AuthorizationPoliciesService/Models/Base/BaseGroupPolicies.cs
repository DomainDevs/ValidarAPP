using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseGroupPolicies : Extension
    {
        [DataMember]
        public int IdGroupPolicies { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public string Key { set; get; }

        [DataMember]
        public string EntityDescription { set; get; }
    }
}
