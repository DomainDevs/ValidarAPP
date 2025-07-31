using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup
{
    [DataContract]
    public class Economicgroupoperatingquota
    {
        [DataMember]
        public int EconomicGroupID { get; set; }

        [DataMember]
        public string EconomicGroupName { get; set; }

        [DataMember]
        public bool Enable { get; set; }

        [DataMember]
        public decimal ValueOpQuota { get; set; }

        [DataMember]
        public DateTime InitDate { get; set; }

        [DataMember]
        public DateTime DeclineDate { get; set; }
    }
}
