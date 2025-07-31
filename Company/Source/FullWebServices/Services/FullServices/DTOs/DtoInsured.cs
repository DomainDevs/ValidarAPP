using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.DTOs;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class DtoInsured
    {

        [DataMember]
        public DtoDataInsured dtoDataInsured { get; set; }

        [DataMember]
        public DtoDataPerson dtoDataPerson { get; set; }

        [DataMember]
        public DtoRep dtoRep { get; set; }


        [DataMember]
        public DtoSarlaft dtoSarlaft { get; set; }
        
        [DataMember]
        public List<Mpersona_dir> List_Mpersona_dir { get; set; }

        [DataMember]
        public List<Mpersona_telef> List_Mpersona_telef { get; set; }

        [DataMember]
        public List<Mpersona_email> List_Mpersona_email { get; set; }

        [DataMember]
        public List<LOGBOOK> List_Logbook { get; set; }
        
        [DataMember]
        public List<Mcesionario> List_Mcesionario { get; set; }        

        [DataMember]
        public List<Frm_sarlaft_accionistas_asoc> List_Frm_sarlaft_accionistas_asoc { get; set; }

        [DataMember]
        public List<Maseg_asociacion> List_Maseg_asociacion { get; set; }

        [DataMember]
        public List<Maseg_pmin_gastos_emi> List_Maseg_pmin_gastos_emi { get; set; }

        [DataMember]
        public List<Maseg_tasa_tarifa> List_Maseg_tasa_tarifa { get; set; }

        [DataMember]
        public List<Referencias> List_Referencias { get; set; }        

        [DataMember]
        public List<Mpersona_cuentas_bancarias> List_Mpersona_cuentas_bancarias { get; set; }

        [DataMember]
        public List<Maseg_conducto> List_Maseg_conducto { get; set; }

        [DataMember]
        public DtoTechnicalCard dtoTechnicalCard { get; set; }

        [DataMember]
        public List<INDIVIDUAL_TAX_EXEMPTION> List_INDIVIDUAL_TAX_EXEMPTION { get; set; }
    }
}
