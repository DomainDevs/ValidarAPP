// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Sistran.Company.Application.ModelServices.Enums
{
    /// <summary>
    /// Estados del objeto
    /// </summary>
    [DataContract]
    public enum StatusTypeService
    {
        /// <summary>
        /// Estado original
        /// </summary>
        [EnumMember]
        Original = 1,

        /// <summary>
        /// Creación del objeto
        /// </summary>
        [EnumMember]
        Create = 2,

        /// <summary>
        /// Actualización del objeto
        /// </summary>
        [EnumMember]
        Update = 3,

        /// <summary>
        /// Eliminación del objeto
        /// </summary>
        [EnumMember]
        Delete = 4,

        /// <summary>
        /// Error en el proceso
        /// </summary>
        [EnumMember]
        Error = 5
    }
}