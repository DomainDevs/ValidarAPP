// -----------------------------------------------------------------------
// <copyright file="SubModuleViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="SubModuleViewModel" />
    /// </summary>
    public class SubModuleViewModel
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion
        /// </summary>
        public int DescriptionModule { get; set; }
    }
}