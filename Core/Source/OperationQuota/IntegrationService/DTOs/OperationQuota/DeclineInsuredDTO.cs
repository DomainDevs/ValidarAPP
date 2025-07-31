using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class DeclineInsuredDTO
    {
        [DataMember]
        public bool Decline { get; set; }

        [DataMember]
        public DateTime DeclineDate { get; set; }
    }
}
