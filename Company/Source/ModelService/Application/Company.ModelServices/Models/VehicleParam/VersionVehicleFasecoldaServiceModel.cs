// -----------------------------------------------------------------------
// <copyright file="VersionVehicleFasecoldaModelService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.VehicleParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de  servicio para CO_VEHICLE_VERSION_FASECOLDA
    /// </summary>
    [DataContract]
    public class VersionVehicleFasecoldaServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// id de la version del vehiculo
        /// </summary>
        [DataMember]
        public int VersionId { get; set; }

        /// <summary>
        /// id del modelo del vehiculo
        /// </summary>
        [DataMember]
        public int ModelId { get; set; }

        /// <summary>
        /// Id de la marca del vehiculo
        /// </summary>
        [DataMember]
        public int MakeId { get; set; }

        /// <summary>
        /// Codio de modelo fasecolda del vehiculo
        /// </summary>
        [DataMember]
        public string FasecoldaModelId { get; set; }

        /// <summary>
        /// Codigo de marca fasecolda del vehiculo
        /// </summary>
        [DataMember]
        public string FasecoldaMakeId { get; set; }
    }
}
