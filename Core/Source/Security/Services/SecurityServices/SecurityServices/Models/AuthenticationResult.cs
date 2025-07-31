using Sistran.Core.Application.SecurityServices.Enums;
using Sistran.Core.Application.SecurityServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.Models
{
    [DataContract]
    public class AuthenticationResult : BaseAuthenticationResult
    {
        [DataMember]
        public AuthenticationEnum Result { get; set; }
        [DataMember]
        public List<object> data { get; set; }
    }
}
