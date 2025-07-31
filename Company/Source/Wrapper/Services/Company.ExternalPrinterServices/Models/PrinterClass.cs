using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class PrinterClass
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string PrinterBinary { get; set; }
        [DataMember]
        public string XMLFTP { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
