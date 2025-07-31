using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DataContract]
    public class VehiclePolicyExtend
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public decimal DocumentNum { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
    }
}
