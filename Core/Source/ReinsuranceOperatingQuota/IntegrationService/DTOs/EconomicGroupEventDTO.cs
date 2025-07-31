using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs
{
    [DataContract]
    public class EconomicGroupEventDTO
    {
        [DataMember]
        public int EconomicGroupEventID { get; set; }

        [DataMember]
        public int EconomicGroupEventEventType { get; set; }

        [DataMember]
        public int EconomicGroupID { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public string payload { get; set; }

        [DataMember]
        public EconomicgroupoperatingquotaDTO economicgroupoperatingquotaDTO { get; set; }

        [DataMember]
        public EconomicgrouppartnersDTO economicgrouppartnersDTO { get; set; }
    }
}
