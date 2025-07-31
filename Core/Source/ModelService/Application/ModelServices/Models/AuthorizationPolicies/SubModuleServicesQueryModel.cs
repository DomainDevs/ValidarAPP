// -----------------------------------------------------------------------
// <copyright file="SubModuleServicesQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="SubModuleServicesQueryModel" />
    /// </summary>
    [DataContract]
    public class SubModuleServicesQueryModel
    {
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ModuleId
        /// </summary>
        [DataMember]
        public int? ModuleId { get; set; }
    }
}
