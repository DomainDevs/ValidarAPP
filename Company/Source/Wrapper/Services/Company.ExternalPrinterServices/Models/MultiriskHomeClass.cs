using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class MultiriskHomeClass
    {
        [DataMember]
        public string Id_vivienda { get; set; }
        [DataMember]
        public string Tipo_Vivienda { get; set; }
        [DataMember]
        public string Direccion_vivienda { get; set; }
        [DataMember]
        public string Cod_ciudad_vivienda { get; set; }
        [DataMember]
        public string Desc_ciudad_vivienda { get; set; }
        [DataMember]
        public string Cod_departamento_vivienda { get; set; }
        [DataMember]
        public string Desc_departamento_vivienda { get; set; }
        [DataMember]
        public string Cod_pais_vivienda { get; set; }
        [DataMember]
        public string Desc_pais_vivienda { get; set; }
        [DataMember]
        public string Valor_contenido { get; set; }
        [DataMember]
        public string Valor_vivienda { get; set; }
    }
}
