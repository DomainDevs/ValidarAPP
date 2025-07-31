#region Using

using System;
using System.Runtime.Serialization;
//using Sistran.Core.Application.CommonService.DTOs;

#endregion

namespace Sistran.Core.Application.TempCommonServices.DTOs
{
    [DataContract]
    public class EndorsementDTO 
    {
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public int InsuredCd { get; set; }
        [DataMember]
        public string InsuredName { get; set; }
        [DataMember]
        public string OperationType { get; set; }
        [DataMember]
        public decimal Prime { get; set; }
        [DataMember]
        public decimal InsuredAmount { get; set; }
        [DataMember]
        public decimal ResponsibilityMaximumAmount { get; set; }
    }
}
