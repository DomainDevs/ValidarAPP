using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("System.Xml", "4.0.30319.1015")]
    [XmlType(Namespace = "http://fasecolda.org.co/")]
    [DataContract]
    public class HistoricoPolizaCexper
    {
        [DataMember]
        public string PPH { get; set; }
        [DataMember]
        public string PH { get; set; }
        [DataMember]
        public string PPD { get; set; }
        [DataMember]
        public string PTD { get; set; }
        [DataMember]
        public double ValorAsegurado { get; set; }
        [DataMember]
        public string Asegurado { get; set; }
        [DataMember]
        public string NumeroDocumento { get; set; }
        [DataMember]
        public string TipoDocumentoAsegurado { get; set; }
        [DataMember]
        public string Servicio { get; set; }
        [DataMember]
        public short Modelo { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string RC { get; set; }
        [DataMember]
        public string Clase { get; set; }
        [DataMember]
        public string CodigoGuia { get; set; }
        [DataMember]
        public string Vigente { get; set; }
        [DataMember]
        public DateTime FechaFinVigencia { get; set; }
        [DataMember]
        public DateTime FechaVigencia { get; set; }
        [DataMember]
        public string Chasis { get; set; }
        [DataMember]
        public string Motor { get; set; }
        [DataMember]
        public string Placa { get; set; }
        [DataMember]
        public long Orden { get; set; }
        [DataMember]
        public string NumeroPoliza { get; set; }
        [DataMember]
        public string NombreCompania { get; set; }
        [DataMember]
        public short CodigoCompania { get; set; }
        [DataMember]
        public string Marca { get; set; }
        [DataMember]
        public string TipoCruce { get; set; }
    }        
}
