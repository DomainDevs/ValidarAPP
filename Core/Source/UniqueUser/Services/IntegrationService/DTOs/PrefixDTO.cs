using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
   public class PrefixDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
        [DataMember]
        public bool HasDetailedCommission { get; set; }
        [DataMember]
        public int PrefixTypeCode { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int LineBusinessId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public PrefixTypeDTO PrefixType { get; set; }
        [DataMember]
        public List<LineBusinessDTO> LineBusiness { get; set; }
    }
}
