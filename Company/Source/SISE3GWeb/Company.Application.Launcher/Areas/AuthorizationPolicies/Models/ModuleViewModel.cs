// -----------------------------------------------------------------------
// <copyright file="ModuleViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ModuleViewModel" />
    /// </summary>
    public class ModuleViewModel
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        public int SubModuleId { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion
        /// </summary>
        public int DescriptionSubModule { get; set; }
    }
}