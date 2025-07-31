// -----------------------------------------------------------------------
// <copyright file="VehicleBodyServiceQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Moreno</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Tipo de carroceria del vehiculo
    /// </summary>
    [DataContract]
    public class VehicleBodyServiceQueryDTO : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el identificador de la carroceria
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de la carroceria
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
