// -----------------------------------------------------------------------
// <copyright file="CoverageHomologation2GViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Homologacion 2G de Coverturas
    /// </summary>
    public class CoverageHomologation2GViewModel
    {
        /// <summary>
        /// Obtiene o establece el id de la covertura de 2G
        /// </summary>
        public int CoverageId2G { get; set; }

        /// <summary>
        /// Obtiene o establece el id del objeto de seguro de 2g
        /// </summary>
        public int InsuredObject2G { get; set; }

        /// <summary>
        /// Obtiene o establece la linea de negocio de 2G
        /// </summary>
        public int LineBusiness2G { get; set; }

        /// <summary>
        /// Obtiene o establece la linea de negocio de 2G
        /// </summary>
        public int SubLineBusiness2G { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del registro
        /// </summary>
        public int Status { get; set; }
    }
}