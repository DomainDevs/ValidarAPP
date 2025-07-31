using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
   public class DtoDataDC
    {
        [DataMember]
        public Tdirector_comercial tdirector_comercial { get; set; }

        [DataMember]
        public Mpersona mpersona { get; set; }

        [DataMember]
        public Mpersona_ciiu mpersona_ciiu { get; set; }

        [DataMember]
        public Mpersona_requiere_sarlaft mpersona_requiere_sarlaft { get; set; }

        [DataMember]
        public Tdirector_comercial_hist tdirector_comercial_hist { get; set; }

    }
}
