using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UnderwritingOperatingQuotaServices.DTOs.EconomicGroup
{
    [DataContract]
    public class EconomicgrouppartnersDTO
    {
        [DataMember]
        public int EconomicGroupId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public bool Enable { get; set; }

        [DataMember]
        public DateTime InitDate { get; set; }

        [DataMember]
        public DateTime DeclineDate { get; set; }
    }
}
