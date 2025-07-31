using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class GroupDelegationDTO : GenericListDTO
    {
        [DataMember]
        public int HierarchyId { set; get; }
    }
}
