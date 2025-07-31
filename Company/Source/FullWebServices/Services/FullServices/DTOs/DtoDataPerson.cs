using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System.Collections.Generic;

namespace Sistran.Co.Previsora.Application.FullServices.DTOs
{
    [DataContract]
    public class DtoDataPerson
    {
        [DataMember]
        public Mpersona mpersona { get; set; }
        
        [DataMember]
        public Maseg_header maseg_header { get; set; }

        [DataMember]
        public Tipo_persona_aseg tipo_persona_aseg { get; set; }

        [DataMember]
        public Maseg_ficha_tec_financ maseg_ficha_tec_financ { get; set; }

        [DataMember]
        public List<Maseg_deporte> listMaseg_deporte { get; set; }

    }
}
