// -----------------------------------------------------------------------
// <copyright file="CoveragesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------   ------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para lista de coberturas
    /// </summary>
    public class CoveragesClauseServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de coberturas
        /// </summary>
        [DataMember]
        public List<CoverageClauseServiceModel> CoverageServiceModels { get; set; }
    }
}
