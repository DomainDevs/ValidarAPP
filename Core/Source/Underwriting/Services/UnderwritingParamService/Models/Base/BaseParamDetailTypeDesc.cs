// -----------------------------------------------------------------------
// <copyright file="ParamDetailTypeDesc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio de tipos de detalle
    /// </summary>
    [DataContract]
    public class BaseParamDetailTypeDesc: Extension
    {
        /// <summary>
        /// Obtiene o establece el id del tipo de detalle
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del tipo de detalle
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si eldeducible es obligatorio
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
