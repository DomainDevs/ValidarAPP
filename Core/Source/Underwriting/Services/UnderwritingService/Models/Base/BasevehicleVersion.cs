using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BasevehicleVersion
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// VehicleBodyCode
        /// </summary>
        [DataMember]
        public int VehicleBodyCode { get; set; }

        /// <summary>
        /// VehicleTypeCode
        /// </summary>
        [DataMember]
        public int VehicleTypeCode { get; set; }

        /// <summary>
        /// IsImported
        /// </summary>
        [DataMember]
        public bool IsImported { get; set; }

        /// <summary>
        /// VehicleFuelCode
        /// </summary>
        [DataMember]
        public int VehicleFuelCode { get; set; }

        /// <summary>
        /// EngineCylQuantity
        /// </summary>
        [DataMember]
        public int EngineCylQuantity { get; set; }

        /// <summary>
        /// EngineCc
        /// </summary>
        [DataMember]
        public int EngineCc { get; set; }

        /// <summary>
        /// EngineTypeCode
        /// </summary>
        [DataMember]
        public int EngineTypeCode { get; set; }

        /// <summary>
        /// TransmissionTypeCode
        /// </summary>
        [DataMember]
        public int TransmissionTypeCode { get; set; }

        /// <summary>
        /// Horsepower
        /// </summary>
        [DataMember]
        public int Horsepower { get; set; }

        /// <summary>
        /// TopSpeed
        /// </summary>
        [DataMember]
        public int TopSpeed { get; set; }

        /// <summary>
        /// DoorQuantity
        /// </summary>
        [DataMember]
        public int DoorQuantity { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        [DataMember]
        public int Weight { get; set; }

        /// <summary>
        /// PassengerQuantity
        /// </summary>
        [DataMember]
        public int PassengerQuantity { get; set; }

        /// <summary>
        /// TonsQuantity
        /// </summary>
        [DataMember]
        public int TonsQuantity { get; set; }

        /// <summary>
        /// NewVehiclePrice
        /// </summary>
        [DataMember]
        public decimal NewVehiclePrice { get; set; }

        /// <summary>
        /// CurrencyCode
        /// </summary>
        [DataMember]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// IaVehicleVersionCode
        /// </summary>
        [DataMember]
        public int IaVehicleVersionCode { get; set; }

        /// <summary>
        /// ServiceId
        /// </summary>
        [DataMember]
        public int ServiceId { get; set; }

        /// <summary>
        /// ServiceDescription
        /// </summary>
        [DataMember]
        public string ServiceDescription { get; set; }

        /// <summary>
        /// PartialLossBaseValue
        /// </summary>
        [DataMember]
        public int PartialLossBaseValue { get; set; }

        /// <summary>
        /// Nationality
        /// </summary>
        [DataMember]
        public string Nationality { get; set; }

        /// <summary>
        /// CurrencyCode
        /// </summary>
        [DataMember]
        public bool AirConditioning { get; set; }

        /// <summary>
        /// VehicleAxleQuantity
        /// </summary>
        [DataMember]
        public int VehicleAxleQuantity { get; set; }        
        
        /// <summary>
        /// CurrencyCode
        /// </summary>
        [DataMember]
        public string CodeStatus { get; set; }

        /// <summary>
        /// LastModel
        /// </summary>
        [DataMember]
        public bool LastModel { get; set; }

        /// <summary>
        /// WeightCategoryCode
        /// </summary>
        [DataMember]
        public int WeightCategoryCode { get; set; }

        /// <summary>
        /// NoveltyCode
        /// </summary>
        [DataMember]
        public string NoveltyCode { get; set; }

        /// <summary>
        /// ApprovedFasecoldaModelId
        /// </summary>
        [DataMember]
        public string ApprovedFasecoldaModelId { get; set; }

        /// <summary>
        /// ApprovedFasecoldaMakeId
        /// </summary>
        [DataMember]
        public string ApprovedFasecoldaMakeId { get; set; }
    }
}
