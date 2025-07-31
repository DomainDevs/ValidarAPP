using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class ListEndorsementPolicyClass
    {
        [DataMember]
        public string Cod_Suc { get; set; }
        [DataMember]
        public string Desc_Suc { get; set; }
        [DataMember]
        public string Cod_Ramo { get; set; }
        [DataMember]
        public string Desc_Ramo { get; set; }
        [DataMember]
        public string Nro_Poliza { get; set; }
        [DataMember]
        public string Nro_Endoso { get; set; }
        [DataMember]
        public string Desc_Endoso { get; set; }
        [DataMember]
        public string Fec_emi { get; set; }
        [DataMember]
        public string Fec_vig_desde { get; set; }
        [DataMember]
        public string Fec_vig_hasta { get; set; }
        [DataMember]
        public string Vlr_Aseg_Endoso { get; set; }
        [DataMember]
        public string Vlr_Prima_Endoso { get; set; }
    }
}
