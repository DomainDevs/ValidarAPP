using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class TypePolicies : Extension
    {
        [DataMember]
        public int IdTypePolicies { set; get; }

        [DataMember]
        public string Description { set; get; }
    }
}
