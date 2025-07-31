using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class AssociationLine
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
        public int LineBusiness { get; set; }

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
    }
}
