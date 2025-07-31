using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseRiskVehicle
    {
        [DataMember]
        public int VehicleYear { get; set; }

        [DataMember]
        public decimal VehiclePrice { get; set; }

        [DataMember]
        public bool IsNew { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string EngineNumber { get; set; }

        [DataMember]
        public string ChassisNumber { get; set; }

        [DataMember]
        public int LoadTypeId { get; set; }

        [DataMember]
        public int TrailersQuantity { get; set; }

        [DataMember]
        public int PassengersQuantity { get; set; }

        [DataMember]
        public decimal NewVehiclePrice { get; set; }

        [DataMember]
        public decimal StandardVehiclePrice { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }
    }
}
