using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DataContract]
    public class RenewallHistory
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string KeyId { get; set; }
        [DataMember]
        public string IdCardTypeCode { get; set; }
        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public DateTime EndoCurrentFrom { get; set; }
        [DataMember]
        public DateTime EndoCurrentTo { get; set; }
        [DataMember]
        public string NewRenewall { get; set; }
        [DataMember]
        public decimal? RenewallNum { get; set; }
        [DataMember]
        public string Chanel { get; set; }
        [DataMember]
        public decimal Distance { get; set; }
        [DataMember]
        public DateTime DateDataIni { get; set; }
        [DataMember]
        public DateTime DateDataEnd { get; set; }
        [DataMember]
        public DateTime LoadDate { get; set; }
    }
}