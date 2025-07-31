using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    public class PolicyInfo : PrintingInfo
    {
        [DataMember]
        public int PolicyId;
        [DataMember]
        public int EndorsementId;
        [DataMember]
        public int? NumberReport;
        [DataMember]
        public int? TicketNumber;
        [DataMember]
        public int? BranchId;
        [DataMember]
        public int? PolicyNumber;
        [DataMember]
        public int? EndorsementNum;
        [DataMember]
        public bool? IsCollective;
        [DataMember]
        public CommonProperties CommonProperties;
        [DataMember]
        public bool EndorsementText;
        [DataMember]
        public bool CurrentFromFirst;
    }
}
