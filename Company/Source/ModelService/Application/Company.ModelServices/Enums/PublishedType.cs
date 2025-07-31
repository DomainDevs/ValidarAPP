// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ModelServices.Enums
{
    [DataContract]
    [Flags]
    public enum PublishedType
    {
        /// <summary>
        /// Constante porcentaje
        /// </summary>
        [EnumMember]
        Todas = 1,

        /// <summary>
        /// Constante pormilaje
        /// </summary>        
        [EnumMember]
        Publicada = 2,

        /// <summary>
        /// Constante valor fijo
        /// </summary>        
        [EnumMember]
        SinPublicar = 3
    }
}
