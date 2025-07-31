using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO
{
    [DataContract]
    public class ListRiskLoadDTO
    {
        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public DateTime BeginDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }

        [DataMember]
        public ListRiskDTO ListRisk { get; set; }

        [DataMember]
        public int Event { get; set; }

        [DataMember]
        public ProcessStatusEnum ProcessStatus { get; set; }
    }
}
