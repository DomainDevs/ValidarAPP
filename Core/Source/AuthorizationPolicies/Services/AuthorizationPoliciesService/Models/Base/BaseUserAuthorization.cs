using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseUserAuthorization : Extension
    {
        [DataMember]
        public bool Default { set; get; }

        [DataMember]
        public bool Required { set; get; }
    }
}
