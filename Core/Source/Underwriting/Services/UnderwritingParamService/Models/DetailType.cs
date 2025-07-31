using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    [DataContract]
    public class DetailType
    {
            [DataMember]
            public int DetailTypeCode { get; set; }

            [DataMember]
            public string Description { get; set; }

            [DataMember]
            public string SmallDescription { get; set; }

            [DataMember]
            public int DetailClassCode { get; set; }
        
    }
}
