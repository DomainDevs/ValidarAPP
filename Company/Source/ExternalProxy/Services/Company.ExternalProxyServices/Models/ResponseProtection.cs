namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseProtection
    {
        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public string ClaimNumber { get; set; }

        [DataMember]
        public DateTime ClaimDate { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string ProtectedName { get; set; }

        [DataMember]
        public double ProtectedClaimValue { get; set; }

        [DataMember]
        public double ProtectedPaidValue { get; set; }

        [DataMember]
        public DateTime NoticeDate { get; set; }
    }
}