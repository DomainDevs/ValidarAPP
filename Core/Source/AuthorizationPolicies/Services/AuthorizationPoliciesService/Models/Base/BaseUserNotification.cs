using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models.Base
{
    [DataContract]
    public class BaseUserNotification : Extension
    {
        [DataMember]
        public bool Default { set; get; }
    }
}
