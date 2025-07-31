// -----------------------------------------------------------------------
// <copyright file="TechnicalPlansServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Modelo de servicio de consulta de los planes Técnicos.
    /// </summary>
    [DataContract]
    public class TechnicalPlansServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Propiedad de la Lista de planes Técnicos.
        /// </summary>
        [DataMember]
        public List<TechnicalPlanServiceModel> TechnicalPlanServiceModel { get; set; }
    }
}
