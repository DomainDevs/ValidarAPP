using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class GetConsulSinisterPolicyRequest
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
        public string Fecdesdesin { get; set; }
        [DataMember]
        public string Fechastasin { get; set; }
        [DataMember]
        public string Fecdesdeavisin { get; set; }
        [DataMember]
        public string Fechastaavisin { get; set; }
        [DataMember]
        public string Fecdesdepagsin { get; set; }
        [DataMember]
        public string Fechastapagsin { get; set; }       
    }
}
