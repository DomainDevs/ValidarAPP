using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class IndividualOperatingQuotaDTO
    {
        [DataMember]
        public int IndividualID { get; set; }

        [DataMember]
        public int LineBusinessID { get; set; }

        [DataMember]
        public decimal ValueOpQuotaAMT { get; set; }

        [DataMember]
        public int ParticipationPercentage { get; set; }

        [DataMember]
        public DateTime InitDateOpQuota { get; set; }

        [DataMember]
        public DateTime EndDateOpQuota { get; set; }
    }
}
