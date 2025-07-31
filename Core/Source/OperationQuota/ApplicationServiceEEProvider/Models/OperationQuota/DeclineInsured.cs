using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class DeclineInsured
    {
        [DataMember]
        public bool Decline { get; set; }

        [DataMember]
        public DateTime DeclineDate { get; set; }
    }
}
