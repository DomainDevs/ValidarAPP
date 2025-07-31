// -----------------------------------------------------------------------
// <copyright file="ErrorTypeService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------


using System.Runtime.Serialization;

namespace Sistran.Company.Application.ModelServices.Enums
{
    /// <summary>
    /// Tipos de error
    /// </summary>
    [DataContract]
    public enum ErrorTypeService
    {
        /// <summary>
        /// Respuesta satisfactoria
        /// </summary>
        [EnumMember]
        Ok = 0,

        /// <summary>
        /// No se encontraron registros
        /// </summary>
        [EnumMember]
        NotFound = 1,

        /// <summary>
        /// Falla técnica
        /// </summary>
        [EnumMember]
        TechnicalFault = 2,

        /// <summary>
        /// FAlla de negocio
        /// </summary>
        [EnumMember]
        BusinessFault = 3
    }
}
