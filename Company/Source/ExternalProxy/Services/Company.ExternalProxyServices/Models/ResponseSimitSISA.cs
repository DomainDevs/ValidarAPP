namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseSimitSISA
    {
        [DataMember]
        public string InfringementCode { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public DateTime PenaltyDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string Secretary { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }
}