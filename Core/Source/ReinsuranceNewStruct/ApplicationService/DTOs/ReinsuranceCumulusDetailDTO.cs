using Sistran.Core.Application.ReinsuranceServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsuranceCumulusDetailDTO
    {
        [DataMember]
        public int Id{ get; set; }
        [DataMember]
        public decimal RetentionAmount { get; set; }
        [DataMember]
        public decimal AssigmentAmount { get; set; }
        [DataMember]
        public CumulusDetailDTO CumulusDetail { get; set; }
    }
}
