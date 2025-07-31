using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    public class QuotationInfo : PrintingInfo
    {
        [DataMember]
        public int QuotationId;

        [DataMember]
        public int VersionId;

        [DataMember]
        public int TempId;

        [DataMember]
        public CommonProperties CommonProperties;
    }
}
