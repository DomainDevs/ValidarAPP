using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BasePoliciesAut : Extension
    {
        [DataMember]
        public int IdPolicies { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public int Position { set; get; }

        [DataMember]
        public int IdHierarchyPolicy { set; get; }

        [DataMember]
        public int IdHierarchyAut { set; get; }

        [DataMember]
        public int NumberAut { set; get; }

        [DataMember]
        public string Message { set; get; }

        [DataMember]
        public bool Enabled { set; get; }
    }
}
