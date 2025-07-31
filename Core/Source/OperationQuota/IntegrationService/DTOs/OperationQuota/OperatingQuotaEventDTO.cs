using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota
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

        public DateTime Policy_Init_Date { get; set; }

        [DataMember]
        public DateTime Policy_End_Date { get; set; }

        [DataMember]
        public DateTime Cov_Init_Date { get; set; }

        [DataMember]
        public DateTime? Cov_End_Date { get; set; }

        [DataMember]
        public DeclineInsuredDTO declineInsured { get; set; }
        //JSON
        [DataMember]
        public IndividualOperatingQuotaDTO IndividualOperatingQuota { get; set; }

        [DataMember]
        public ApplyEndorsementDTO ApplyEndorsement { get; set; }

        [DataMember]
        public ApplyReinsuranceDTO ApplyReinsurance { get; set; }

        public ConsortiumEventDTO consortiumEventDTO { get; set; }

        [DataMember]
        public EconomicGroupEventDTO EconomicGroupEventDTO { get; set; }
        [DataMember]
        public int PrefixCd { get; set; }
    }
}
