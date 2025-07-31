using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class GetConsulRecoveriesPolicyRequest
    {
        [DataMember]
        public int Codsuc { get; set; }
        [DataMember]
        public int Codramo { get; set; }
        [DataMember]
        public decimal Numpoliza { get; set; }
        [DataMember]
        public int Numendoso { get; set; }
        [DataMember]
        public string Fecinimora { get; set; }
        [DataMember]
        public string Fecfinmora { get; set; }
        [DataMember]
        public string Fecdesdeultpago { get; set; }
        [DataMember]
        public string Fechastaultpago { get; set; }
        [DataMember]
        public string Fecdesdeproxpago { get; set; }
        [DataMember]
        public string Fechastaproxpago { get; set; }
    }
}
