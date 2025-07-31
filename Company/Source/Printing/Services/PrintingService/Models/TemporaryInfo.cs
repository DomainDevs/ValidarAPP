using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    public class TemporaryInfo : PrintingInfo
    {
        [DataMember]
        public int TempId;

        [DataMember]
        public int OperationId;

        [DataMember]
        public bool? IsCollective;

        [DataMember]
        public CommonProperties CommonProperties;

        [DataMember]
        public bool TempAuthorization;
    }
}
