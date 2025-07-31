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
{    /// <summary>
     /// Modelo de vehiculo
     /// </summary>
    [DataContract]
    public class VehicleModelsServiceQueryModel : ErrorServiceModel
    {    /// <summary>
         /// se obtiene listado
         /// </summary>
        [DataMember]
        public List<VehicleModelServiceQueryModel> VehicleModelServiceQueryModel { get; set; }
    }
}