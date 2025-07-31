using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseScoreCredit
    {
        [DataMember]
        public string Score { get; set; }

        [DataMember]
        public DateTime? Date { get; set; }

        [DataMember]
        public Error Error { get; set; }
    }
}
