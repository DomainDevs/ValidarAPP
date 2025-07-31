// -----------------------------------------------------------------------
// <copyright file="KeyType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.EntityServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum KeyType
    /// </summary>
    [DataContract]
    public enum KeyType
    {
        /// <summary>
        /// Enumerador ninguno
        /// </summary>
        [EnumMember]
        None = 1,

        /// <summary>
        /// Autonumber: Parameter
        /// </summary>
        [EnumMember]
        Autonumber = 2,

        /// <summary>
        /// enumerador identity
        /// </summary>
        [EnumMember]
        Identity = 3,

        /// <summary>
        /// enumerador identity llave compuesta
        /// </summary>
        [EnumMember]
        IdentityByKey = 4,

        /// <summary>
        /// enumerador identity llave compuesta
        /// </summary>
        [EnumMember]
        NextValue = 5
    }
}
