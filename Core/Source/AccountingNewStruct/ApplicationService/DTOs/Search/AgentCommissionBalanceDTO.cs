using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AgentCommissionBalanceDTO 
    {
        [DataMember]
        public int AgentCommissionBalanceCode { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public string DocumentNumAgent { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
    }
}
