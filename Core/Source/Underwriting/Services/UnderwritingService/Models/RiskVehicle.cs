using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class RiskVehicle : BaseRiskVehicle
    {
        [DataMember]
        public VehicleVersion VehicleVersion { get; set; }

        [DataMember]
        public VehicleModel VehicleModel { get; set; }

        [DataMember]
        public VehicleMake VehicleMake { get; set; }

        [DataMember]
        public Risk Risk { get; set; }

        [DataMember]
        public VehicleType VehicleType { get; set; }

        [DataMember]
        public VehicleUse VehicleUse { get; set; }

        [DataMember]
        public VehicleBody VehicleBody { get; set; }

        [DataMember]
        public VehicleColor VehicleColor { get; set; }

        [DataMember]
        public VehicleFuel VehicleFuel { get; set; }

    }
}
