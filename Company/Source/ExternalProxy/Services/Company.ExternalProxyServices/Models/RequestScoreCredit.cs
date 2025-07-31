using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class RequestScoreCredit
    {
        [DataMember]
        public int DocumentTypeDataCredit { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public Guid GuidProcess { get; set; }
    }
}
