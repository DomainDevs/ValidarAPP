using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ExternalInfrigement
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string InfringementState { get; set; }
        [DataMember]
        public DateTime InfringementDate { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string LicensePlate {get; set;}
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public DateTime DateRequest { get; set; }


    }
}
