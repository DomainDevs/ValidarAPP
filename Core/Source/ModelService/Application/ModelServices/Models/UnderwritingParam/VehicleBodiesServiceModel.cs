// -----------------------------------------------------------------------
// <copyright file="VehicleBodiesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Listados de carroceria de vehiculo
    /// </summary>
    [DataContract]
    public class VehicleBodiesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado las carroceria de vehiculos
        /// </summary>
        [DataMember]
        public List<VehicleBodyServiceModel> VehicleBodyServiceModel { get; set; }
    }
}
