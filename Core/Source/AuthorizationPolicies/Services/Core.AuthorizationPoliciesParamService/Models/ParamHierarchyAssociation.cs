// -----------------------------------------------------------------------
// <copyright file="ParamHierarchyAssociation.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ParamHierarchyAssociation" />
    /// </summary>
    [DataContract]
    public class ParamHierarchyAssociation
    {
        /// <summary>
        /// Obtiene o establece la Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si IsEnabled
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si IsExclusionary
        /// </summary>
        [DataMember]
        public bool IsExclusionary { get; set; }

        /// <summary>
        /// Obtiene o establece modelo ParamHierarchy
        /// </summary>
        [DataMember]
        public ParamHierarchy ParamHierarchy { get; set; }

        /// <summary>
        /// Obtiene o establece modelo ParamModulen
        /// </summary>
        [DataMember]
        public ParamModule ParamModulen { get; set; }

        /// <summary>
        /// Obtiene o establece modelo ParamSubModule
        /// </summary>
        [DataMember]
        public ParamSubModule ParamSubModule { get; set; }
    }
}
