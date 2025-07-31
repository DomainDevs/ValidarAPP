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
    /// Tipo Marca
    /// </summary>
    [DataContract]
    public class VehicleMakesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// se obtiene identificador
        /// </summary>
        [DataMember]
        public List<VehicleMakeServiceQueryModel> VehicleMakeServiceQueryModel { get; set; }
    }
}