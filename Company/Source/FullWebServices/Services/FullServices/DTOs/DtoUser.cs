using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.DTOs;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class DtoUser
    {
        [DataMember]
        public DtoDataUser dtoDataUser { get; set; }

        [DataMember]
        public DtoDataPerson dtoDataPerson { get; set; }

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
        public List<Tusuario_limites> List_Tusuario_limites { get; set; }

        [DataMember]
        public List<Tusuario_modulo_imputacion> List_Tusuario_modulo_imputacion { get; set; }

        [DataMember]
        public List<Tusuario_concepto> List_Tusuario_concepto { get; set; }

        [DataMember]
        public List<Tusuario_centro_costo> List_Tusuario_centro_costo { get; set; }

        [DataMember]
        public List<Tpj_usuarios_email> List_Tpj_usuarios_email { get; set; }

    }
}
