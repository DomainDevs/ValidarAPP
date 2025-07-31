// -----------------------------------------------------------------------
// <copyright file="VehicleTypesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Listados de tipos de vehiculo
    /// </summary>
    [DataContract]
    public class VehicleTypesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado los tipos de vehiculos
        /// </summary>
        [DataMember]
        public List<VehicleTypeServiceModel> VehicleTypeServiceModel { get; set; }
    }
}
