using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class DtoTechnicalAssistant
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public int IdApp { get; set; }

        [DataMember]
        public DtoDataTA dtoDataTA { get; set; }

        [DataMember]
        public Mpersona Mpersona { get; set; }

        [DataMember]
        public List<Mpersona_dir> List_Mpersona_dir { get; set; }

        [DataMember]
        public List<Mpersona_telef> List_Mpersona_telef { get; set; }

        [DataMember]
        public List<Mpersona_email> List_Mpersona_email { get; set; }

        [DataMember]
        public List<LOGBOOK> List_Logbook { get; set; }

        [DataMember]
        public List<Frm_sarlaft_accionistas_asoc> List_Frm_sarlaft_accionistas_asoc { get; set; }

        [DataMember]
        public DtoSarlaft dtoSarlaft { get; set; }
       
    }
}
