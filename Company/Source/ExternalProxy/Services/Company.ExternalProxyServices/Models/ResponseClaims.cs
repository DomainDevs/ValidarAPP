namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseClaims
    {
        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string ClaimNumber { get; set; }

        [DataMember]
        public string PolicyNumber { get; set; }

        [DataMember]
        public double Order { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string Engine { get; set; }

        [DataMember]
        public string Chassis { get; set; }

        [DataMember]
        public DateTime ClaimDate { get; set; }

        [DataMember]
        public string GuiedCode { get; set; }

        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public string Class { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Model { get; set; }

        [DataMember]
        public string InsuredDocumentType { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public double InsuredAmount { get; set; }

        [DataMember]
        public string InterchangeType { get; set; }

        [DataMember]
        public List<ResponseProtection> Protection { get; set; }
    }
}