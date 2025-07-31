using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class DetailCumulusParticipantsEconomicGroupDTO
    {
        [DataMember]
        public EconomicGroupDTO EconomicGroup { get; set; }

        [DataMember]
        public EconomicGroupDetailDTO EconomicGroupDetail { get; set; }

        [DataMember]
        public InsuredDTO Insured { get; set; }

        [DataMember]
        public bool Enable { get; set; }

        [DataMember]
        public DateTime DateUpdated { get; set; }

        [DataMember]
        public decimal TotalCumulusIndividual { get; set; }

        [DataMember]
        public decimal TotalCumulusEconomicGroup { get; set; }

        [DataMember]
        public decimal RetentionTotalCumulusIndividual { get; set; }

        [DataMember]
        public decimal AssignmentTotalCumulusIndividual { get; set; }

        [DataMember]
        public decimal TotalPremiumsIndividual { get; set; }

    }
}
