// -----------------------------------------------------------------------
// <copyright file="ParamCoveredRiskTypeDesc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    /// <summary>
    /// Modelo de negocio de grupo de coberturas
    /// </summary>
    public class BaseParamCoveredRiskTypeDesc: Extension
    {
        /// <summary>
        /// Obtiene o establece el id de grupo de coberturas
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de grupo de coberturas
        /// </summary>
        public string Description { get; set; }
    }
}
