// -----------------------------------------------------------------------
// <copyright file="FasecoldaDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Moreno</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Listado Tipo de carroceria del vehiculo
    /// </summary>
    [DataContract]
    public class VehicleBodysServiceQueryDTO : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado de la carroceria
        /// </summary>
        [DataMember]
        public List<VehicleBodyServiceQueryDTO> VehicleBodyServiceQueryModel { get; set; }
    }
}
