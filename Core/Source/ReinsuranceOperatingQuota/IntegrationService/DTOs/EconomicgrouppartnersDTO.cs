using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs
{
    [DataContract]
    public class EconomicgrouppartnersDTO
    {
        [DataMember]
        public int EconomicGroupId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public DateTime InitDate { get; set; }

        [DataMember]
        public DateTime DeclineDate { get; set; }
    }
}
