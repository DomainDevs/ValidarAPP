// -----------------------------------------------------------------------
// <copyright file="ParamSubModule.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ParamSubModule" />
    /// </summary>
    [DataContract]
    public class ParamSubModule
    {
        /// <summary>
        /// Obtiene o establece la Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del modulo
        /// </summary>
        [DataMember]
        public Nullable<int> ModuleId { get; set; }
    }
}
