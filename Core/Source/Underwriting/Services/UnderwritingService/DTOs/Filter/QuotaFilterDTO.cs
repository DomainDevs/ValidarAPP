using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.DTOs.Filter
{
    [DataContract]
    public class QuotaFilterDTO
    {
        [DataMember]
        public int PlanId { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public ComponentValueDTO ComponentValueDTO { get; set; }
    }
}
