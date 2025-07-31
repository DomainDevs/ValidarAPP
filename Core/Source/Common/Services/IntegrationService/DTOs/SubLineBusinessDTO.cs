using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.CommonServices.DTOs
{
    [DataContract]
    public class SubLineBusinessDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public int LineBusinessId { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string LineBusinessDescription { get; set; }
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }
    }
}
