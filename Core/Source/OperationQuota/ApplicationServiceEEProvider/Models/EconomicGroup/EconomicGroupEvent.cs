using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup
{
    [DataContract]
    public class EconomicGroupEvent
    {
        [DataMember]
        public int EconomicGroupEventId { get; set; }

        [DataMember]
        public int EconomicGroupEventType { get; set; }

        [DataMember]
        public int EconomicGroupId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public string payload { get; set; }

        [DataMember]
        public DeclineInsured declineInsured{get;set;}

        [DataMember]
        public Economicgroupoperatingquota EconomicGroupOperatingQuota { get; set; }

        [DataMember]
        public Economicgrouppartners EconomicGroupPartners { get; set; }
    }
}
            