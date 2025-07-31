// -----------------------------------------------------------------------
// <copyright file="VersionServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para Version de Hehiculo
    /// </summary>
    [DataContract]
    public class VehicleVersionServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public VehicleMakeServiceQueryModel VehicleMakeServiceQueryModel { get; set; }
        [DataMember]
        public VehicleModelServiceQueryModel VehicleModelServiceQueryModel { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? EngineQuantity { get; set; }
        [DataMember]
        public int? HorsePower { get; set; }
        [DataMember]
        public int? Weight { get; set; }
        [DataMember]
        public int? TonsQuantity { get; set; }
        [DataMember]
        public int? PassengerQuantity { get; set; }
        [DataMember]
        public VehicleFuelServiceQueryModel VehicleFuelServiceQueryModel { get; set; }
        [DataMember]
        public VehicleBodyServiceQueryModel VehicleBodyServiceQueryModel { get; set; }
        [DataMember]
        public VehicleTypeServiceQueryModel VehicleTypeServiceQueryModel { get; set; }
        [DataMember]
        public VehicleTransmissionTypeServiceQueryModel VehicleTransmissionTypeServiceQueryModel { get; set; }

        [DataMember]
        public int? MaxSpeedQuantity { get; set; }

        [DataMember]
        public int? DoorQuantity { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public bool IsImported { get; set; }
        [DataMember]
        public bool? LastModel { get; set; }
        [DataMember]
        public CurrencyServiceQueryModel CurrencyServiceQueryModel { get; set; }


    }
}