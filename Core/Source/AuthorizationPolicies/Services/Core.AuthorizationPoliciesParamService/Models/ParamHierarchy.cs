// -----------------------------------------------------------------------
// <copyright file="ParamHierarchy.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ParamHierarchy" />
    /// </summary>
    [DataContract]
    public class ParamHierarchy
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
    }
}
