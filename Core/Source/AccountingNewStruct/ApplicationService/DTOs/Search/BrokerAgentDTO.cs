using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class BrokerAgentDTO 
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentCode { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public DateTime DeclinedDate { get; set; }
    }
}
