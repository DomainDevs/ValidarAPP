using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.Models.Base
{
    [DataContract]
    public class BaseAuthenticationResult : Extension
    {
        [DataMember]
        public bool isAuthenticated { get; set; }

        [DataMember]
        public int UserId { get; set; }
    }
}
