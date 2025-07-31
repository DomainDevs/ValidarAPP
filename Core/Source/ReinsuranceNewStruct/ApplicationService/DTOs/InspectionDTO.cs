using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class InspectionDTO
    {
        [DataMember]
        public int AdjusterId { get; set; }
        [DataMember]
        public int AnalizerId { get; set; }
        [DataMember]
        public int ResearcherId { get; set; }
        [DataMember]
        public DateTime RegistrationDate { get; set; }
        [DataMember]
        public string RegistrationHour { get; set; }
        [DataMember]
        public string AffectedProperty { get; set; }
        [DataMember]
        public string LossDescription { get; set; }
    }
}