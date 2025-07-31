using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs
{
    [DataContract]
    public class FasecoldaDTO
    {
        [DataMember]
        public string FasecoldaCode { get; set; }
        [DataMember]
        public string MakeId { get; set; }
        [DataMember]
        public string ModelId { get; set; }
        [DataMember]
        public string VersionId { get; set; }
    }
}
