using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseConceptDescription : Extension
    {
        [DataMember]
        public string Name { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public string Value { set; get; }

        [DataMember]
        public int Order { set; get; }
    }
}
