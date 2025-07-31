// -----------------------------------------------------------------------
// <copyright file="HierarchiesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UniqueUser
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="HierarchiesServiceQueryModel" />
    /// </summary>
    public class HierarchiesServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece lista de Jerarquias
        /// </summary>
        public List<SubModuleServicesQueryModel> SubModuleServiceQueryModels { get; set; }
    }
}
