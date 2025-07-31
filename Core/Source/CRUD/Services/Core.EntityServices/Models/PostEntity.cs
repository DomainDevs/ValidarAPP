// -----------------------------------------------------------------------
// <copyright file="PostEntity.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.EntityServices.Models
{
    using Sistran.Core.Application.EntityServices.Enums;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase PostEntity
    /// </summary>
    [DataContract]
    public class PostEntity
    {
        /// <summary>
        /// Obtiene o establece el tipo entidad
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string EntityType { get; set; }

        /// <summary>
        /// Obtiene o establece los campos
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<Field> Fields { get; set; }

        /// <summary>
        /// Obtiene o establece el estado
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public StatusTypeService? Status { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de llave
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public KeyType? KeyType { get; set; }
    }
}
