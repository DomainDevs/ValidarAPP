using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class ComponentType
    {
        [DataMember]
        public int ComponentTypeId { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
    }
}
