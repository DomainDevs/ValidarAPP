using System;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class ResquestSearch
    {
        [DataMember(EmitDefaultValue = false)]
        public String cod_Rol { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Int32? idRol { get; set; }
        [DataMember]
        public String nro_documento { get; set; }
        [DataMember]
        public Int32? tipo_doc { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember]
        public string ID_user { get; set; }
    }
}
