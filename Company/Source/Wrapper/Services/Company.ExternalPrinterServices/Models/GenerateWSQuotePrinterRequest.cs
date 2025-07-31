using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class GenerateWSQuotePrinterRequest
    {
        [DataMember]
        public int TempId { get; set; }
        [DataMember]
        public string EmailUser { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool PrintBinary { get; set; }
        [DataMember]
        public bool? JustRecordQuote { get; set; }
    }
}
