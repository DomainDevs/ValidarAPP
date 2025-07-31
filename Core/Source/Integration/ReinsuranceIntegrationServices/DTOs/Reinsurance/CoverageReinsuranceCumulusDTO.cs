using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class CoverageReinsuranceCumulusDTO
    {
        [DataMember]
        public int PolicyID { get; set; }
        [DataMember]
        public DateTime PolicyCurrentFrom { get; set; }
        [DataMember]
        public DateTime PolicyCurrentTo { get; set; }
        [DataMember]
        public int DocumentNum { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int EndorsementType { get; set; }
        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public DateTime CoverageCurrentFrom { get; set; }
        [DataMember]
        public DateTime CoverageCurrentTo { get; set; }
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public CurrencyDTO Currency{ get; set; }
        [DataMember]
        public InsuredDTO Insured { get; set; }
        [DataMember]
        public InsuredDTO Consortium { get; set; }
        [DataMember]
        public EconomicGroupDTO EconomicGroup { get; set; }
        [DataMember]
        public ContractReinsuranceCumulusDTO ContractReinsuranceCumulus { get; set; }
        [DataMember]
        public CoverageDTO Coverage { get; set; }




    }
}
