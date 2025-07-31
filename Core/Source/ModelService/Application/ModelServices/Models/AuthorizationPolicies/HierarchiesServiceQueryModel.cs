// -----------------------------------------------------------------------
// <copyright file="HierarchiesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="HierarchiesServiceQueryModel" />
    /// </summary>}
    [DataContract]
    public class HierarchiesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de Jerarquias
        /// </summary>
        [DataMember]
        public List<HierarchyServiceQueryModel> HierarchyServiceQueryModels { get; set; }
    }
}
