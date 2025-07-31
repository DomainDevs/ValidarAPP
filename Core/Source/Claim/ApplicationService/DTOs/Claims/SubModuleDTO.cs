using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class SubModuleDTO
    {
        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public int SubModuleId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
}
