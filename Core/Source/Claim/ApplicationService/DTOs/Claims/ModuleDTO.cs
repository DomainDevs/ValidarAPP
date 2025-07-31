using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ModuleDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public string EnabledDescription { get; set; }

        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        [DataMember]
        public string VirtualFolder { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
