// -----------------------------------------------------------------------
// <copyright file="ParametrizacionInsuredObject.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Objetos del seguro (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class BaseParamInsuredObject: Extension
    {
        /// <summary>
        /// Obtiene o establece el ID de objetos del seguro
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de objetos del seguro
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion abreviada
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si
        /// </summary>
        [DataMember]
        public bool IsDeclarative { get; set; }               
    }
}
