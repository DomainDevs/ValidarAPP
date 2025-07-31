using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AircraftServices.DTOs
{
    [DataContract]
    public class AircraftTypeDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
