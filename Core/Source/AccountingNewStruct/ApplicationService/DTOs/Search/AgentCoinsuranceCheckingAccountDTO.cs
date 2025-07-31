using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AgentCoinsuranceCheckingAccountDTO 
    {
        [DataMember]
        public int AgentCoinsuranceCheckingAccountCode { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentCode { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public decimal CommissionAmount { get; set; }
        [DataMember]
        public decimal IncomeCommissionAmount { get; set; }
    }
}
