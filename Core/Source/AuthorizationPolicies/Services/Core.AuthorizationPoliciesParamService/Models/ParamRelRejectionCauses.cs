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
    public class ParamRelRejectionCauses
    {
        /// <summary>
        /// Obtiene o establece ParamBaseEjectionCauses
        /// </summary>
        [DataMember]
        public ParamBaseEjectionCauses ParamBaseEjectionCauses { get; set; }

        /// <summary>
        /// Obtiene o establece ParamBaseEjectionCauses
        /// </summary>
        [DataMember]
        public ParamBaseGroupPolicies ParamBaseGroupPolicies { get; set; }
    }
}
