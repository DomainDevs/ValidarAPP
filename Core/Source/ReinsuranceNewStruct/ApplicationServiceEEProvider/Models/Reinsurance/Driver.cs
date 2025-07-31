using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Driver
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClaimCoverageId { get; set; }

        [DataMember]
        public string LicenseType { get; set; }

        [DataMember]
        public string LicenseNumber { get; set; }

        [DataMember]
        public DateTime LicenseValidThru { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}