// -----------------------------------------------------------------------
// <copyright file="HierarchiesAssociationServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;
    
    /// <summary>
    /// Defines the <see cref="HierarchiesAssociationServiceModel" />
    /// </summary>
    [DataContract]
    public class HierarchiesAssociationServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de clausula
        /// </summary>
        [DataMember]
        public List<HierarchyAssociationServiceModel> HierarchyAssociationServiceModel { get; set; }
    }
}
