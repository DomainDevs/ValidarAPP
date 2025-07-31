// -----------------------------------------------------------------------
// <copyright file="FieldType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.EntityServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase field type
    /// </summary>
    [DataContract]
    public class FieldType
    {
        /// <summary>
        /// Obtiene o establece el nombre del tipo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si permite multiples
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool Multiple { get; set; }
    }
}
