// -----------------------------------------------------------------------
// <copyright file="FasecoldaServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.VehicleParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de Fasecolda
    /// </summary>
    [DataContract]
    public class FasecoldaServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// version del vehiculo
        /// </summary>
        [DataMember]
        public VersionServiceModel Version { get; set; }

        /// <summary>
        /// modelo del vehiculo
        /// </summary>
        [DataMember]
        public ModelServiceModel Model { get; set; }

        /// <summary>
        /// marca del vehiculo
        /// </summary>
        [DataMember]
        public MakeServiceModel Make { get; set; }

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
