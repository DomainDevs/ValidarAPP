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
    public class OperatingQuotalog
    {
        [DataMember]
        public double OldOperatingQuotaAmt { get; set; }

        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public DateTime TransactionDate { get; set; }

        [DataMember]
        public int IsWs { get; set; }
    }
}
