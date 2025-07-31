// -----------------------------------------------------------------------
// <copyright file="StatusTypeService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Fernando Méndez Cardoso</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.EntityServices.Enums
{
    /// <summary>
    /// Estados del objeto
    /// </summary>
    public enum StatusTypeService
    {
        /// <summary>
        /// Estado original
        /// </summary>
        Original = 1,

        /// <summary>
        /// Creación del objeto
        /// </summary>
        Create = 2,

        /// <summary>
        /// Actualización del objeto
        /// </summary>
        Update = 3,

        /// <summary>
        /// Eliminación del objeto
        /// </summary>
        Delete = 4,

        /// <summary>
        /// Error en el proceso
        /// </summary>
        Error = 5
    }
}
