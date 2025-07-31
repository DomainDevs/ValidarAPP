using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class DtoSarlaft
    {
        [DataMember]
        public List<Frm_sarlaft_detalle> List_detalle { get; set; }

        [DataMember]
        public Frm_sarlaft_detalle_entrevista frm_sarlaft_detalle_entrevista { get; set; }

        [DataMember]
        public Frm_sarlaft_info_financiera frm_sarlaft_info_financiera { get; set; }

        [DataMember]
        public List<Frm_sarlaft_oper_internac> List_oper_internacionales { get; set; }

        [DataMember]
        public Frm_sarlaft_vinculos frm_sarlaft_vinculos { get; set; }

        [DataMember]
        public Frm_sarlaft_aut_incrementos frm_sarlaft_aut_incrementos { get; set; }
       
    }
}
