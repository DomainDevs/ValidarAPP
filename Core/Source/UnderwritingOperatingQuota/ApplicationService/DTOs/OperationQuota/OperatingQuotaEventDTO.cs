using Sistran.Core.Application.UnderwritingOperatingQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.UnderwritingOperatingQuotaServices.DTOs.EconomicGroup;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingOperatingQuotaServices.DTOs.OperationQuota
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
        public string payload { get; set; }
        [DataMember]
        public DateTime Policy_Init_Date { get; set; }

        [DataMember]
        public DateTime Policy_End_Date { get; set; }

        [DataMember]
        public DateTime Cov_Init_Date { get; set; }

        [DataMember]
        public DateTime Cov_End_Date { get; set; }

        //JSON
        [DataMember]
        public IndividualOperatingQuotaDTO IndividualOperatingQuota { get; set; }

        [DataMember]
        public ApplyEndorsementDTO ApplyEndorsement { get; set; }

        [DataMember]
        public ApplyReinsuranceDTO ApplyReinsurance { get; set; }

        [DataMember]
        public ConsortiumEventDTO consortiumEventDTO { get; set; }

        [DataMember]
        public EconomicGroupEventDTO EconomicGroupEventDTO { get; set; }
    }
}
