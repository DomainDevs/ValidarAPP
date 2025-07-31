using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class ConsultDataBasicPolicyResponse
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
        public string Tipo_doc { get; set; }
        [DataMember]
        public string Nro_doc { get; set; }
        [DataMember]
        public string Cod_cli { get; set; }
        [DataMember]
        public string Nombre_cli { get; set; }
        [DataMember]
        public string Apellido1_cli { get; set; }
        [DataMember]
        public string Apellido2_cli { get; set; }
        [DataMember]
        public string Clave_int { get; set; }
        [DataMember]
        public string Nro_endo { get; set; }
        [DataMember]
        public string Anio_emi_endo { get; set; }
        [DataMember]
        public string Tipo_doc_agente { get; set; }
        [DataMember]
        public string Nro_doc_agente { get; set; }
        [DataMember]
        public string Nombre_agente { get; set; }
        [DataMember]
        public string Fec_ini_vig { get; set; }
        [DataMember]
        public string Fec_fin_vig { get; set; }
        [DataMember]
        public string Tipo_pol { get; set; }
        [DataMember]
        public string Placa { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string Marca { get; set; }
        [DataMember]
        public string Nombre_producto { get; set; }
        [DataMember]
        public string Valor_prima { get; set; }
        [DataMember]
        public string Periodicidad_prima { get; set; }
        [DataMember]
        public string Val_aseg { get; set; }
        [DataMember]
        public string Val_ahorro { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
