using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AgentCommissionClosureDTO 
    {
        [DataMember]
        public int AgentCommissionClosureCode { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public int Status { get; set; }
    }
}
