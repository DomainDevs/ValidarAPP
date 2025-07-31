
using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class Prefix : BasePrefix
    {
        [DataMember]
        public PrefixType PrefixType { get; set; }
        [DataMember]
        public List<LineBusiness> LineBusiness { get; set; }
               
    }
}
