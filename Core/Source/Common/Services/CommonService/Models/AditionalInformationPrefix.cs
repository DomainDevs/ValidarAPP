using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class AditionalInformationPrefix
    {
        [DataMember]
        public bool IsScore { get; set; }

        [DataMember]
        public bool IsAlliance { get; set; }

        [DataMember]
        public bool IsMassive { get; set; }

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public int? Quote { get; set; }

        [DataMember]
        public int? Temporal { get; set; }
    }
}
