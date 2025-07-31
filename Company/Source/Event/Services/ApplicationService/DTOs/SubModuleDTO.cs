using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class SubModuleDTO : GenericListDTO
    {
        [DataMember]
        public int ModuleId { get; set; }
    }
}
