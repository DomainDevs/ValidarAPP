// -----------------------------------------------------------------------
// <copyright file="Field.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.EntityServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase field
    /// </summary>
    [DataContract]
    public class Field
    {
        /// <summary>
        /// Obtiene o establece el nombre del campo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public FieldType Type { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si permite nulos
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool AllowNull { get; set; }

        /// <summary>
        /// Obtiene o establece el valor
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Value { get; set; }

        /// <summary>
        /// Obtiene o establece el valor que indica si el campo es llave primaria para otro campo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsKeyForOtherColumn { get; set; }
        /// <summary>
        /// Obtiene o establece el valor que indica si el campo depende de otra columna
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsConsecutiveByKeyOtherColumn { get; set; }
    }
}
