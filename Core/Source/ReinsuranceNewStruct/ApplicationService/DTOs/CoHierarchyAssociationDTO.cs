using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    public class CoHierarchyAssociationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool ExclusionayInd { get; set; }
        [DataMember]
        public bool EnabledInd { get; set; }
        [DataMember]
        public decimal? LimitInsuredAmt { get; set; }

        [DataMember]
        public ModuleDTO Module { get; set; }
        [DataMember]
        public SubModuleDTO SubModule { get; set; }
    }
}
