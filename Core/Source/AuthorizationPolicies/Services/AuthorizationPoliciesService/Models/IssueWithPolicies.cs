// -----------------------------------------------------------------------
// <copyright file="IssueWithPolicies.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    using System;
    using System.Runtime.Serialization;

    public class IssueWithPolicies
    {
        /// <summary>
        /// Atributo para la propiedad TemporalId.
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Atributo para la propiedad EndorsementType.
        /// </summary>
        [DataMember]
        public string EndorsementType { get; set; }

        /// <summary>
        /// Atributo para la propiedad DocumentNumber.
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Atributo para la propiedad Branch.
        /// </summary>
        [DataMember]
        public string Branch { get; set; }

        /// <summary>
        /// Atributo para la propiedad Prefix.
        /// </summary>
        [DataMember]
        public string Prefix { get; set; }

        /// <summary>
        /// Atributo para la propiedad DateRequest.
        /// </summary>
        [DataMember]
        public DateTime DateRequest { get; set; }
    }
}