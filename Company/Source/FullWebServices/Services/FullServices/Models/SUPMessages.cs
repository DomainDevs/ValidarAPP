using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract] 
    public class SUPMessages
    {
        [DataMember]
        public String cod_rol { get; set; }

        [DataMember]
        public String id_persona { get; set; }

        [DataMember]
        public String INDIVIDUAL_ID { get; set; }

        [DataMember]
        public List<TableMessage> ListMessages { get; set; }       
    }
}
