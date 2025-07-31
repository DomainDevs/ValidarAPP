
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class DependenciesDTO
    {
        [DataMember]
        public string EntityDescription { get; set; }

        [DataMember]
        public string DependsDescription { get; set; }

        [DataMember]
        public string Column { get; set; }

        [DataMember]
        public int EntityId { get; set; }

        [DataMember]
        public int ConditionsId { get; set; }

        [DataMember]
        public int DependesId { get; set; }
    }
}