using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class AssociationLineDTO
    {
        [DataMember]
        public int AssociationLineId { get; set; }
        [DataMember]
        public int LineId { get; set; }
        [DataMember]
        public string LineDescription { get; set; }
        [DataMember]
        public int AssociationColumnId { get; set; }
        [DataMember]
        public int ValueFrom { get; set; }
        [DataMember]
        public int ValueTo { get; set; }
        [DataMember]
        public int SubValueFrom { get; set; }
        [DataMember]
        public int SubValueTo { get; set; }
        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public string DateTo { get; set; }
        [DataMember]
        public int AssociationTypeId { get; set; }
        [DataMember]
        public int LineBusiness { get; set; } //Prefix
        [DataMember]
        public string LineBusinessDescriptionFrom { get; set; }
        [DataMember]
        public string LineBusinessDescriptionTo { get; set; }
        [DataMember]
        public string SubLineBusinessDescriptionFrom { get; set; }
        [DataMember]
        public string SubLineBusinessDescriptionTo { get; set; }
        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public ByLineBusinessDTO ByLineBusiness{ get; set; }
        [DataMember]
        public ByLineBusinessSubLineBusinessDTO ByLineBusinessSubLineBusiness { get; set; }
        [DataMember]
        public ByOperationTypePrefixDTO ByOperationTypePrefix { get; set; }
        [DataMember]
        public ByInsuredDTO ByInsured { get; set; }
        [DataMember]
        public ByPrefixDTO ByPrefix { get; set; }
        [DataMember]
        public ByPolicyDTO ByPolicy { get; set; }
        [DataMember]
        public ByFacultativeIssueDTO ByFacultativeIssue { get; set; }
        [DataMember]
        public ByInsuredPrefixDTO ByInsuredPrefix { get; set; }
        
        [DataMember]
        public ByPrefixProductDTO ByPrefixProduct { get; set; }

        [DataMember]
        public ByLineBusinessSubLineBusinessRiskDTO ByLineBusinessSubLineBusinessRisk { get; set; }
        [DataMember]
        public ByPrefixRiskDTO ByPrefixRisk { get; set; }
        [DataMember]
        public ByPolicyLineBusinessSubLineBusinessDTO ByPolicyLineBusinessSubLineBusiness { get; set; }
        [DataMember]
        public ByLineBusinessSubLineBusinessCoverageDTO ByLineBusinessSubLineBusinessCoverage { get; set; }
        

    }
}