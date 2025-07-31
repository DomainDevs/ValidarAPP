using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimDocumentationDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public int SubmoduleId { get; set; }

        [DataMember]
        public int prefix { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsRequired { get; set; }

        [DataMember]
        public bool? Enable { get; set; }
    }
}
