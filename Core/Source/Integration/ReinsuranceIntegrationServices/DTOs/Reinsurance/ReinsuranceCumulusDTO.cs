using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    public class ReinsuranceCumulusDTO
    {
        [DataMember]
        public List<ContractReinsuranceCumulusDTO> ContractReinsuranceCumulusDTOs { get; set; }
        [DataMember]
        public List<CoverageReinsuranceCumulusDTO> CoverageReinsuranceCumulusDTOsByIndividual { get; set; }
        [DataMember]
        public List<CoverageReinsuranceCumulusDTO> CoverageReinsuranceCumulusDTOsByConsortium { get; set; }
        [DataMember]
        public List<CoverageReinsuranceCumulusDTO> CoverageReinsuranceCumulusDTOsByEconomicGroup { get; set; }
        [DataMember]
        public List<ReinsuranceCumulusDetailDTO> ReinsuranceCumulusDetailDTOs { get; set; }
        [DataMember]
        public decimal TotalCumulus { get; set; }
        [DataMember]
        public decimal RetentionTotalCumulus { get; set; }
        [DataMember]
        public decimal AssignmentTotalCumulus { get; set; }
    }
}
