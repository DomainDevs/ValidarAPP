using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class ListSinisterPolicyClass
    {
        [DataMember]
        public string Cod_suc { get; set; }
        [DataMember]
        public string Nom_suc { get; set; }
        [DataMember]
        public string Cod_Ramo { get; set; }
        [DataMember]
        public string Desc_Ramo_Com { get; set; }
        [DataMember]
        public string Nro_pol { get; set; }
        [DataMember]
        public string Nro_Endoso { get; set; }
        [DataMember]
        public string Txt_desc_endo { get; set; }
        [DataMember]
        public string Nro_Reclamo { get; set; }
        [DataMember]
        public string Cod_Ramo_tec { get; set; }
        [DataMember]
        public string Desc_Ramo_Tec { get; set; }
        [DataMember]
        public string Sucursal_reclamo { get; set; }
        [DataMember]
        public string Fec_siniestro { get; set; }
        [DataMember]
        public string Fecha_Aviso { get; set; }
        [DataMember]
        public string Fecha_pago { get; set; }
        [DataMember]
        public string Vlr_pagado { get; set; }
        [DataMember]
        public string Clase_pago { get; set; }
        [DataMember]
        public string Nro_riesgo_sin { get; set; }
        [DataMember]
        public string Desc_Amparo { get; set; }
    }
}
