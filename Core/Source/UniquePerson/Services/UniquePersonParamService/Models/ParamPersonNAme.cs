// -----------------------------------------------------------------------
// <copyright file="ParamPerson.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gomez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamService.Models
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de Persona
    /// </summary>
    public class ParamPerson
    {
        /// <summary>
        /// Obtiene o establece el PersonId
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Obtiene o establece la nombre
        /// </summary>
        public string Name { get; set; }
    }
}
