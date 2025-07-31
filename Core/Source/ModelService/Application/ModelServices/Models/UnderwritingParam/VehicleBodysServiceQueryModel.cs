// -----------------------------------------------------------------------
// <copyright file="VehicleBodyServiceQueryModel.cs" company="SISTRAN">
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
    /// Listado Tipo de carroceria del vehiculo
    /// </summary>
    [DataContract]
    public class VehicleBodysServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado de la carroceria
        /// </summary>
        [DataMember]
        public List<VehicleBodyServiceQueryModel> VehicleBodyServiceQueryModel { get; set; }
    }
}
