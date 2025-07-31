using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Inspection
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