using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class UserGroupModel : Extension
    {
        [DataMember]
        public int UserId { set; get; }

        [DataMember]
        public int GroupId { set; get; }
    }
}
