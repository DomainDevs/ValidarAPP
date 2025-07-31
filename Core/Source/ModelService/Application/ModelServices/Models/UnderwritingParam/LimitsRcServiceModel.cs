// -----------------------------------------------------------------------
// <copyright file="LimitRcModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para limite Rc
    /// </summary>
    [DataContract]
    public class LimitsRcServiceModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// Propiedad de la Lista de tipos de riesgo cubierto.
        /// </summary>
        [DataMember]
        public List<LimitRcServiceModel> LimitRcModel { get; set; }
    }
}
