using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    [DataContract]
    public class CompanyListRiskLoad : CompanyListRiskMassiveLoad
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime BeginDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public bool HasError { get; set; }
        
        [DataMember]
        public string Error_Description { get; set; }

        [DataMember]
        public ProcessStatusEnum ProcessStatus { get; set; }

        [DataMember]
        public int RiskListType { get; set; }

        [DataMember]
        public int Event { get; set; }
    }
}
