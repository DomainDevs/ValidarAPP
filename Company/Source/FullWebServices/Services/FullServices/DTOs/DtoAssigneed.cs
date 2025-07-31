using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.DTOs;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class DtoAssigneed
    {
        [DataMember]
        public DtoDataAssigneed DtoDataAssigneed { get; set; }

        [DataMember]
        public List<Mpersona_dir> List_Mpersona_dir { get; set; }

        [DataMember]
        public List<Mpersona_telef> List_Mpersona_telef { get; set; }

        [DataMember]
        public List<Mpersona_email> List_Mpersona_email { get; set; }

        [DataMember]
        public List<LOGBOOK> List_Logbook { get; set; }

        [DataMember]
        public DtoSarlaft dtoSarlaft { get; set; }
        
        [DataMember]
        public List<Mcesionario> List_Mcesionario { get; set; }

        [DataMember]
        public List<Frm_sarlaft_accionistas_asoc> List_Frm_sarlaft_accionistas_asoc { get; set; }                        

        [DataMember]
        public List<Referencias> List_Referencias { get; set; }
        
        [DataMember]
        public List<Mpersona_cuentas_bancarias> List_Mpersona_cuentas_bancarias { get; set; }
                
        [DataMember]
        public DtoRep dtoRep { get; set; }

        [DataMember]
        public List<CesionarioDe> List_CesionarioDe { get; set; }

        [DataMember]
        public List<Mcesio_trans_bancarias> List_Mcesio_trans_bancarias { get; set; }        

    }
}
