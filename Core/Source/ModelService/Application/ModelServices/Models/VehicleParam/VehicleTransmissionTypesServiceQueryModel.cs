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
    /// Listado de Tipo de transmision
    /// </summary>
    [DataContract]
    public class VehicleTransmissionTypesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Se obtiene listado
        /// </summary>
        [DataMember]
        public List<VehicleTransmissionTypeServiceQueryModel> VehicleTransmissionTypeServiceQueryModel { get; set; }
    }
}