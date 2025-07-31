using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class RiskChangeText
    {
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public int InsuredId { get; set; }
        [DataMember]
        public int CoveredRiskTypeCode { get; set; }
        [DataMember]
        public string ConditionText { get; set; }
        [DataMember]
        public int? RatingZoneCode { get; set; }
        [DataMember]
        public int? CoverGroupId { get; set; }
        [DataMember]
        public bool IsFacultative { get; set; }
        [DataMember]
        public int? NameNum { get; set; }
        [DataMember]
        public int? AddressId { get; set; }
        [DataMember]
        public int? PhoneId { get; set; }
        [DataMember]
        public int? RiskCommercialClassCode { get; set; }
        [DataMember]
        public int? RiskCommercialTypeCode { get; set; }
        [DataMember]
        public int? SecondaryInsuredId { get; set; }
    }
}
