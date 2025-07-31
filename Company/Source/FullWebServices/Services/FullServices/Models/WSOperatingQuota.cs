using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [Serializable]
    [DataContract]
    public class WSOperatingQuota
    {
        [DataMember]
        public int codTipoId { set; get; }

        [DataMember]
        public string identificacion { set; get; }

        [DataMember]
        public double cupoOperativo { get; set; }

        [DataMember]
        public DateTime fechaVencimiento { get; set; }

        [DataMember]
        public DateTime fechaCambio { get; set; }
    }
}
