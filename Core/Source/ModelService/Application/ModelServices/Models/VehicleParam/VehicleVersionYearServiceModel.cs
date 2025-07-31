// -----------------------------------------------------------------------
// <copyright file="VehicleVersionYearServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using System.Runtime.Serialization;

    /// <summary>
    /// Valor por año vehículos
    /// </summary>
    [DataContract]
    public class VehicleVersionYearServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Marca del vehiculo
        /// </summary>
        [DataMember]
        public VehicleMakeServiceQueryModel VehicleMakeServiceQueryModel { get; set; }

        /// <summary>
        /// Modelo del vehiculo
        /// </summary>
        [DataMember]
        public VehicleModelServiceQueryModel VehicleModelServiceQueryModel { get; set; }

        /// <summary>
        /// Version del vehiculo
        /// </summary>
        [DataMember]
        public VehicleVersionServiceQueryModel VehicleVersionServiceQueryModel { get; set; }

        /// <summary>
        /// Moneda usada para el precio de vehiculo
        /// </summary>
        [DataMember]
        public CurrencyServiceQueryModel CurrencyServiceQueryModel { get; set; }

        /// <summary>
        /// Año del vehiculo
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Precio del vehiculo
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }
    }
}
