// -----------------------------------------------------------------------
// <copyright file="CptAssistanceTypeModelView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Homero Trujillo Trujillo</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    /// <summary>
    /// modelo de vista Tipo de asistencia
    /// </summary>
    public class CptAssistanceTypeModelView
    {
        /// <summary>
        /// Obtiene o establece el id del ramo
        /// </summary>
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece el codigo
        /// </summary>
        public int AssistanceCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el tipo de asistencia esta habiliatada
        /// </summary>
        public bool Enabled { get; set; }
    }
}