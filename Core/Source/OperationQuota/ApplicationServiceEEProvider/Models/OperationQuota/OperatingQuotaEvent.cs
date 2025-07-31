using System;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class OperatingQuotaEvent
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
        public string Payload { get; set; }

        [DataMember]
        public DateTime Policy_Init_Date { get; set; }

        [DataMember]
        public DateTime Policy_End_Date { get; set; }

        [DataMember]
        public DateTime Cov_Init_Date { get; set; }

        [DataMember]
        public DateTime? Cov_End_Date { get; set; }

        [DataMember]
        public IndividualOperatingQuota IndividualOperatingQuota { get; set; }

        [DataMember]
        public ApplyEndorsement ApplyEndorsement { get; set; }

        [DataMember]
        public ApplyReinsurance ApplyReinsurance { get; set; }

        [DataMember]
        public EconomicGroupEvent EconomicGroupEvent { get; set; }

        [DataMember]
        public ConsortiumEvent consortiumEvent { get; set; }

        [DataMember]
        public DeclineInsured declineInsured { get; set; }
        
        [DataMember]
        public int PrefixCd { get; set; }
    }
}
