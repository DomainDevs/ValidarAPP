using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceOperatingQuotaServices.DTOs
{   
    [DataContract]
    public class OperatingQuotaEventDTO
    {
        [DataMember]
        public int OperatingQuotaEventID { get; set; }
        [DataMember]
        public int OperatingQuotaEventType { get; set; }
        [DataMember]
        public int IdentificationId { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public int LineBusinessID { get; set; }
        [DataMember]
        public int? SubLineBusinessID { get; set; }
        [DataMember]
        public string payload { get; set; }
        [DataMember]
        public DateTime Policy_Init_Date { get; set; }
        [DataMember]
        public DateTime Policy_End_Date { get; set; }
        [DataMember]
        public DateTime Cov_Init_Date { get; set; }
        [DataMember]
        public DateTime Cov_End_Date { get; set; }
        [DataMember]
        public ApplyReinsuranceDTO ApplyReinsurance { get; set; }
        [DataMember]
        public int PrefixCd { get; set; }
    }
}
