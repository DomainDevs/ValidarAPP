using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class ConsultDataClientResponse
    {
        [DataMember]
        public string Tipo_Doc { get; set; }
        [DataMember]
        public string Nro_Doc { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Primer_Apellido { get; set; }
        [DataMember]
        public string Segundo_Apellido { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public string Tipo_Persona { get; set; }
        [DataMember]
        public string Correo_Electronico { get; set; }
        [DataMember]
        public string Sexo { get; set; }
        [DataMember]
        public string Estado_Civil { get; set; }
        [DataMember]
        public string Departamento { get; set; }
        [DataMember]
        public string Ciudad { get; set; }
        [DataMember]
        public string Codigo_Cliente { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
