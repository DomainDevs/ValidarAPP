using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;

namespace Sistran.Co.Previsora.Application.FullServices.DTOs
{
    [DataContract]
    public class DtoDataInsured
    {
        [DataMember]
        public Mpersona mpersona { get; set; }

        [DataMember]
        public Mpersona_ciiu mpersona_ciiu { get; set; }

        [DataMember]
        public Mpersona_requiere_sarlaft mpersona_requiere_sarlaft { get; set; }

        [DataMember]
        public Maseg_header maseg_header { get; set; }

        [DataMember]
        public Tipo_persona_aseg tipo_persona_aseg { get; set; }

        [DataMember]
        public Tcpto_aseg_adic tcpto_aseg_adic { get; set; }

        [DataMember]
        public Maseg_autoriza_consul maseg_autoriza_consul { get; set; }

        [DataMember]
        public Magente magente { get; set; }

        [DataMember]
        public CO_EQUIVALENCE_INSURED_3G CO_EQUIVALENCE_INSURED_3G { get; set; }
    }    
        
}
