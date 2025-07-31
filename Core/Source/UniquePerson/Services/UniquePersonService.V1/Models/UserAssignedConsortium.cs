using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class UserAssignedConsortium
    {
        [DataMember]
        public int UserAssignedConsortiumId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string NitAssignedConsortium { get; set; }
    }
}
