using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class OperatingQuotaIndividual
    {

        [DataMember]
        public string Connection { get; set; }
        [DataMember]
        public string Document_id { get; set; }
        [DataMember]
        public int Document_type { get; set; }
        [DataMember]
        public int Identity { get; set; }
        [DataMember]
        public int Individual_id { get; set; }
        [DataMember]
        public char State { get; set; }

        public enum OperatingQuotaIndividual_Fields
        {
            Individual_id = 0,
            Document_type = 1,
            Document_id = 2,
        }
    }
}
