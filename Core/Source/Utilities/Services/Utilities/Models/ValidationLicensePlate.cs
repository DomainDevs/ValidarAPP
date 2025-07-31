using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class ValidationLicensePlate : Validation
    {
        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string Engine { get; set; }

        [DataMember]
        public string Chassis { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public DateTime? CurrentTo { get; set; }

        [DataMember]
        public int? ParameterValue { get; set; }
    }
}