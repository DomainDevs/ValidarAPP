// -----------------------------------------------------------------------
// <copyright file="ParamBranch.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.CommonParamService.Models
{
    /// <summary>
    /// Modelo de la sucursal
    /// </summary>
    public class ParamBranch
    {
        /// <summary>
        /// Obtiene o establece el Id de la sucursal
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la sucursal
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta de la sucursal
        /// </summary>
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta de la sucursal
        /// </summary>
        public bool Is_issue { get; set; }
    }
}
