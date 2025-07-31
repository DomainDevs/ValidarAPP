using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    public class IndividualRelationAppDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int RelationTypeId { get; set; }
    }
}
