// -----------------------------------------------------------------------
// <copyright file="ParametricServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Param
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Clase del modelo ParametricServiceModel.
    /// </summary>
    [DataContract]
    public class ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el tipo de error parametrico del modelo de servicio.
        /// </summary>
        [DataMember]
        public ErrorServiceModel ErrorServiceModel { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del tipo de error a retornar en el modelo de servicio.
        /// </summary>
        [DataMember]
        public StatusTypeService StatusTypeService { get; set; }
    }
}
