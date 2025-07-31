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
    /// Operaciones
    /// </summary>
    [DataContract]
    public enum AudictTypeService
    {
        [EnumMember]
        Insert = 1,
        [EnumMember]
        Update = 2,
        [EnumMember]
        Delete = 3
    }
}
