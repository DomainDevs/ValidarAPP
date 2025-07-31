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
    public enum RateType
    {
        /// <summary>
        /// Constante porcentaje
        /// </summary>
        [EnumMember]
        Percentage = 1,

        /// <summary>
        /// Constante pormilaje
        /// </summary>        
        [EnumMember]
        Permilage = 2,

        /// <summary>
        /// Constante valor fijo
        /// </summary>        
        [EnumMember]
        FixedValue = 3
    }
}
