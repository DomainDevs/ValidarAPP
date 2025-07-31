using Sistran.Company.Application.CommonServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class InfringementCount
    {
        [DataMember]
        public GroupInfringement InfringementGroupId { get; set; }

        [DataMember]
        public int InfringementsLastYear { get; set; }

        [DataMember]
        public int InfringementsPeriodOne { get; set; }

        [DataMember]
        public int InfringementsPeriodTwo { get; set; }
    }
}
