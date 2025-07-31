//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;

namespace Sistran.Co.Previsora.Application.FullServices.DTOs
{
    [DataContract]
    public class DtoDataLawyer
    {
        //[DataMember]
        //public string User { get; set; }

        //[DataMember]
        //public int IdApp { get; set; }

        [DataMember]
        public Mabogado mabogado { get; set; }

        [DataMember]
        public Mpersona mpersona { get; set; }

        [DataMember]
        public Mpersona_ciiu mpersona_ciiu { get; set; }

        [DataMember]
        public Mpersona_requiere_sarlaft mpersona_requiere_sarlaft { get; set; }

    }
}
