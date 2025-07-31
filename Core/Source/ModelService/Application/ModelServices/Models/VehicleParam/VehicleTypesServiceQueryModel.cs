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
    /// Tipo de vehiculo
    /// </summary>
    [DataContract]
    public class VehicleTypesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Se obtiene el listado
        /// </summary>
        [DataMember]
        public List<VehicleTypeServiceQueryModel> VehicleTypeServiceQueryModel { get; set; }
    }
}