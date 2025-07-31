// -----------------------------------------------------------------------
// <copyright file="ParamBaseEjectionCauses.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.AuthorizationPoliciesParamService.Models
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.Extensions;

    /// <summary>
    /// Defines the <see cref="ParamBaseEjectionCauses" />
    /// </summary>
    
    [DataContract]
    public class ParamBaseEjectionCauses : Extension
    {
        /// <summary>
        /// Obtiene o establece el ID
        /// </summary>
        [DataMember]
        public  int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public ParamBaseGroupPolicies paramBaseGroupPolicies { get; set; }
    }
}
