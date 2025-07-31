using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class TempReinsuranceProcess
    {
        [DataMember]
        public int TempReinsuranceProcessId { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int RecordsNumber { get; set; }

        [DataMember]
        public int RecordsProcessed { get; set; }

        [DataMember]
        public int RecordsFailed { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string StatusDescription { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        //DETAILS
        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public string PolicyNumber { get; set; }

        [DataMember]
        public int EndorsementNumber { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoverageNumber { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }
        [DataMember]
        public string Progress { get; set; }

        [DataMember]
        public string Elapsed { get; set; }
    }
}
