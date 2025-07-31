using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class ConsultDataClientRequest
    {
        [DataMember]
        public int Tipodoc { get; set; }
        [DataMember]
        public string Nrodoc { get; set; }
        [DataMember]
        public int Codcli { get; set; }
    }
}
