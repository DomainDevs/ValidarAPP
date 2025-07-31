namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseGuideInfo
    {
        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public string Class { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public short Model { get; set; }

        [DataMember]
        public decimal CurrentValue { get; set; }

        [DataMember]
        public decimal InitialValue { get; set; }

        [DataMember]
        public string GuiedDate { get; set; }

        [DataMember]
        public string GuiedCode { get; set; }

        [DataMember]
        public decimal Guied { get; set; }
    }
}
