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
    public class OperatingQuotaObj
    {
        [DataMember]
        public int IndividualId { set; get; }

        [DataMember]
        public int LineBusinessCd { set; get; }

        [DataMember]
        public int CurrencyCd { set; get; }

        [DataMember]
        public double OperatingQuotaAmt { get; set; }

        [DataMember]
        public DateTime CurrentTo { get; set; }

    }
}
