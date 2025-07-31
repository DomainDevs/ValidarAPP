// -----------------------------------------------------------------------
// <copyright file="PartialsInformationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelos de perifericas o hijos 
    /// </summary>
    public class PartialsInformationViewModel
    {
        /// <summary>
        /// Obtiene o establece el id a relacionar
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es obligatorio 
        /// </summary>
        public bool IsMandatory { get; set; }
    }
}