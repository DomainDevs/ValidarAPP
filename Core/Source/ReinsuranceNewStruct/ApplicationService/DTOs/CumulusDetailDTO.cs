using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class CumulusDetailDTO
    {
        [DataMember]
        public String Description { get; set; }
    }
}
