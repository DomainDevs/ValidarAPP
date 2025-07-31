using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;

namespace Sistran.Co.Previsora.Application.FullServices.DTOs
{
    [DataContract]
    public class DtoRep
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public int IdApp { get; set; }  

        [DataMember]
        public Mpersona_rep_legal mpersona_rep_legal { get; set; }

        [DataMember]
        public List<Mpersona_rep_legal_dir> List_mpersona_rep_legal_dir { get; set; }
        
       
    }
}
