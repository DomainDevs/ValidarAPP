using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup
{
    [DataContract]
    public class EconomicGroupDTO
    {
        [DataMember]
        public int EconomicGroupId { get; set; }
        [DataMember]
        public string EconomicGroupName { get; set; }
        [DataMember]
        public int TributaryIdType { get; set; }
        [DataMember]
        public string TributaryIdNo { get; set; }
        [DataMember]
        public int VerifyDigit { get; set; }
        [DataMember]
        public DateTime EnteredDate { get; set; }
        [DataMember]
        public decimal OperationQuoteAmount { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
        [DataMember]
        public int UserId { get; set; }
        public List<EconomicGroupDetailDTO> EconomicGroupDetails { get; set; }
    }
}
