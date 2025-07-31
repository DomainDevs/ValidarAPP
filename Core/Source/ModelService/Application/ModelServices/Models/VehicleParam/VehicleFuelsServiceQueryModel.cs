// -----------------------------------------------------------------------
// <copyright file="VersionServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------

using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    /// <summary>
    /// Listado Tipo de combustible del vehiculo
    /// </summary>
    [DataContract]
    public class VehicleFuelsServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// se obtiene el listado de tipo de combustible
        /// </summary>
        [DataMember]
        public List<VehicleFuelServiceQueryModel> VehicleFuelServiceQueryModel { get; set; }
    }
}