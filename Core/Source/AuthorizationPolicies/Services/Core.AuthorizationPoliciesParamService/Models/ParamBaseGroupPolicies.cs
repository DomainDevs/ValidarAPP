// -----------------------------------------------------------------------
// <copyright file="ParamBaseGroupPolicies.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.AuthorizationPoliciesParamService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ParamBaseGroupPolicies" />
    /// </summary>
    
    [DataContract]
    public class ParamBaseGroupPolicies
    {
        /// <summary>
        /// Obtiene o establece ParamBaseEjectionCauses
        /// </summary>
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// Obtiene o establece ParamBaseEjectionCauses
        /// </summary>
        [DataMember]
        public string description  { get; set; }
    }
}
