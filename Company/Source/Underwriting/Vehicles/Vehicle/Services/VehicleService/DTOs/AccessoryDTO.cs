using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.DTOs
{
    [DataContract]
    public class AccessoryDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public decimal premium { get; set; }
    }
}
