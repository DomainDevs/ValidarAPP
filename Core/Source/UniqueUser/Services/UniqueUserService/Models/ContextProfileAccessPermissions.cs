using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class ContextProfileAccessPermissions
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool Assigned { get; set; } = false;

        [DataMember]
        public SecurityContext SecurityContext { get; set; }

        [DataMember]
        public Profile Profile { get; set; }

        [DataMember]
        public AccessPermissions AccessPermission { get; set; }
    }
}
