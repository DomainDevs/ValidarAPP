using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class AccessPermissions : BaseAccessPermissions
    {
        [DataMember]
        public List<SecurityContext> ContextPermissions { get; set; }
    }
}
