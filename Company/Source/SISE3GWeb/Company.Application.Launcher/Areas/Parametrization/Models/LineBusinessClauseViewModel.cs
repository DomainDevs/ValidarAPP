// -----------------------------------------------------------------------
// <copyright file="LineBusinessClauseViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public class LineBusinessClauseViewModel
    {
        /// <summary>
        /// Obtiene o establece el id de cláusula
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de cláusula
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es obligatoria
        /// </summary>
        public bool Required { get; set; }
    }
}