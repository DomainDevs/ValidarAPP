using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;

namespace Sistran.Co.Previsora.Application.FullServices.DTOs
{
    public class DtoDataUser
    {
        [DataMember]
        public Mpersona mpersona { get; set; }

        [DataMember]
        public Mpersona_usuario mpersona_usuario { get; set; }

        [DataMember]
        public Mpersona_ciiu mpersona_ciiu { get; set; }

        [DataMember]
        public Log_usuario log_usuario { get; set; }

        [DataMember]
        public Tusuario tusuario { get; set; }

        [DataMember]
        public Mpersona_requiere_sarlaft mpersona_requiere_sarlaft { get; set; }

    }
}
