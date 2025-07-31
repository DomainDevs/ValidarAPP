using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.DTO
{
    public class VehicleParameterDTO
    {
        [DataMember]
        public int VehicleMakeCode { get; set; }
        [DataMember]
        public int VehicleModelCode { get; set; }
        [DataMember]
        public int VehicleVersionCode { get; set; }
        [DataMember]
        public int VehicleTypeCode { get; set; }
        [DataMember]
        public string MakeDescription { get; set; }
        [DataMember]
        public string ModelDescription { get; set; }
        [DataMember]
        public string VersionDescription { get; set; }
        [DataMember]
        public string VehicleTypeDescription { get; set; }
        [DataMember]
        public int VehicleYear { get; set; }
        [DataMember]
        public decimal VehiclePrice { get; set; }
        [DataMember]
        public string FasecoldaMakeId { get; set; }
        [DataMember]
        public string FasecoldaModelId { get; set; }
    }
}
