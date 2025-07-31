using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
   public class BranchDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public List<SalePointDTO> SalePoints { get; set; }
    }
}
