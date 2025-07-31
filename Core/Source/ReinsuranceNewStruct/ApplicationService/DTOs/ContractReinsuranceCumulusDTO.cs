using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ContractReinsuranceCumulusDTO
    {
        [DataMember]
        public ContractDTO Contract { get; set; }
        [DataMember]
        public decimal LevelLimit { get; set; }
        [DataMember]
        public decimal AssignmentAmount { get; set; }
        [DataMember]
        public decimal AssignmentAmountLocalCurrency { get; set; }
        [DataMember]
        public decimal AssignmentPremiumAmount { get; set; }
        [DataMember]
        public decimal AssignmentPremiumAmountLocalCurrency { get; set; }
        [DataMember]
        public decimal RetentionAmount { get; set; }
        [DataMember]
        public decimal RetentionPremiumAmount { get; set; }
        [DataMember]
        public decimal RetentionAmountLocalCurrency { get; set; }
        [DataMember]
        public decimal RetentionPremiumAmountLocalCurrency { get; set; }
        [DataMember]
        public decimal AmountLocalCurrency { get; set; }

    }
}
