// -----------------------------------------------------------------------
// <copyright file="ParamModule.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ParamModule" />
    /// </summary>
    [DataContract]
    public class ParamModule
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

        [DataMember]
        public List<ParamSubModule> SubModules { get; set; }
    }
}
