using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class ListRecoveriesPolicyClass
    {
        [DataMember]
        public string Cod_Suc { get; set; }
        [DataMember]
        public string Desc_Sucursal { get; set; }
        [DataMember]
        public string Cod_Ramo { get; set; }
        [DataMember]
        public string Desc_Ramo { get; set; }
        [DataMember]
        public string Nro_Poliza { get; set; }
        [DataMember]
        public string Nro_Endoso { get; set; }
        [DataMember]
        public string Desc_Grupo_Endoso { get; set; }
        [DataMember]
        public string Saldo_en_Mora { get; set; }
        [DataMember]
        public string Fecha_Desde_Mora { get; set; }
        [DataMember]
        public string Fecha_Ultimo_Pago { get; set; }
        [DataMember]
        public string Valor_Ultimo_Pago { get; set; }
        [DataMember]
        public string Fecha_Proximo_Pago { get; set; }
        [DataMember]
        public string Valor_Total_a_Pagar { get; set; }
    }
}
