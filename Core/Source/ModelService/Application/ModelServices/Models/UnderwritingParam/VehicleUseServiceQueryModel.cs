// -----------------------------------------------------------------------
// <copyright file="VehicleUseServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// uso del vehiculo
    /// </summary>
    [DataContract]
    public class VehicleUseServiceQueryModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el identificador del uso
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del uso
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
