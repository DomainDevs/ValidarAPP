// -----------------------------------------------------------------------
// <copyright file="VehicleBodyDTO.cs" company="SISTRAN">
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
    /// carroceria de vehiculo
    /// </summary>
    [DataContract]
    public class VehicleBodyDTO : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Identificador de la carroceria de vehiculo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el listado de usos asociadas a la carroceria de vehiculo
        /// </summary>
        [DataMember]
        public List<VehicleUseServiceQueryDTO> VehicleUseServiceQueryModel { get; set; }

        /// <summary>
        /// Texto del carroceria de vehiculo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string value = string.Empty;
            if (!string.IsNullOrEmpty(SmallDescription))
                value = $"{SmallDescription} ";
            if (Id > 0)
                value += $"({Id}) ";
            if (!string.IsNullOrEmpty(value))
                value += ": ";
            return value;
        }
    }
}
