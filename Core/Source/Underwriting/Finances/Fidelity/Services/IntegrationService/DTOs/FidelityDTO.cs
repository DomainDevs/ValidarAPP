using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.FidelityServices.DTOs
{
    [DataContract]
    public class FidelityDTO
    {
        [DataMember]
        public int CommercialClassId { get; set; }

        [DataMember]
        public string CommercialClassDescription { get; set; }

        [DataMember]
        public int OccupationId { get; set; }

        [DataMember]
        public string OccupationDescription { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime DiscoveryDate { get; set; }

        [DataMember]
        public int? RiskId { get; set; }

        [DataMember]
        public string Risk { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoveredRiskType { get; set; }

        [DataMember]
        public decimal PolicyDocumentNumber { get; set; }

        [DataMember]
        public int? PolicyId { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public int InsuredId { get; set; }

        [DataMember]
        public decimal InsuredAmount { get; set; }
    }
}
