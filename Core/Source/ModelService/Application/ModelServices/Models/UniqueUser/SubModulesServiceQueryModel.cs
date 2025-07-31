// -----------------------------------------------------------------------
// <copyright file="SubModulesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UniqueUser
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SubModulesServiceQueryModel" />
    /// </summary>
    public class SubModulesServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece lista de SubModulos
        /// </summary>
        public List<SubModuleServicesQueryModel> SubModuleServiceQueryModels { get; set; }
    }
}
