using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    [KnownType(typeof(PolicyInfo))]
    [KnownType(typeof(TemporaryInfo))]
    [KnownType(typeof(QuotationInfo))]
    public class PrintingInfo
    {
    }

    [DataContract]
    public class CommonProperties
    {
        [DataMember]
        public string UserName;
        [DataMember]
        public int PrefixId;
        [DataMember]
        public int UserId;
        [DataMember]
        public int RiskSince;
        [DataMember]
        public int RiskUntil;
        [DataMember]
        public bool? IsMassive;
    }
}
