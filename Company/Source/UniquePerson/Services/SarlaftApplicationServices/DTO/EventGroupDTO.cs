using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    public class EventGroupDTO
    {
        [DataMember]
        public int GroupEventId { get; set; }

        [DataMember]
        public int ModuleCode { get; set; }

        [DataMember]
        public int SubmoduleCode { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
