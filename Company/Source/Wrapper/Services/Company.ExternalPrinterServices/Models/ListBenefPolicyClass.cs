using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class ListBenefPolicyClass
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
        public string Tipo_Identificacion { get; set; }
        [DataMember]
        public string Nro_Identificacion { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Apellido1 { get; set; }
        [DataMember]
        public string Apellido2 { get; set; }
        [DataMember]
        public string Direccion_Beneficiario { get; set; }
    }
}
