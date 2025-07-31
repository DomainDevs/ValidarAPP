using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class ConditionGroupDTO : GenericListDTO
    {
        [DataMember]
        public string RelatedEntities { get; set; }

        [DataMember]
        public string RelatedEntitiesId { get; set; }
    }
}
