using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.TempCommonService.DTOs
{
    [DataContract]
    public class PolicyDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public int DocumentNumber { get; set; }
        [DataMember]
        public int EndorsmentId { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public int BusinessType { get; set; }
        [DataMember]
        public int InsuredId { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public EndorsementDTO Endorsement { get; set; }
    }
}
