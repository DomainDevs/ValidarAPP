using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class ListCoveragePolicyClass
    {
        [DataMember]
        public string Cod_suc { get; set; }
        [DataMember]
        public string Desc_suc { get; set; }
        [DataMember]
        public string Cod_ramo { get; set; }
        [DataMember]
        public string Desc_ramo { get; set; }
        [DataMember]
        public string Nro_pol { get; set; }
        [DataMember]
        public string Nro_riesgo { get; set; }
        [DataMember]
        public string Descripcion_Riesgo { get; set; }
        [DataMember]
        public string Cod_cobertura { get; set; }
        [DataMember]
        public string Descripcion_Cobertura { get; set; }
        [DataMember]
        public string Suma_asegurada { get; set; }
        [DataMember]
        public string Suma_asegurada_acum { get; set; }
        [DataMember]
        public string Estado_cobertura { get; set; }
        [DataMember]
        public string Deducible_cobertura { get; set; }
    }
}
