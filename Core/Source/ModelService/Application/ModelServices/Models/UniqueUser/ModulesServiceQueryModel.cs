// -----------------------------------------------------------------------
// <copyright file="ModulesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UniqueUser
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ModulesServiceQueryModel" />
    /// </summary>
    public class ModulesServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece lista de Modulos
        /// </summary>
        public List<ModuleServiceQueryModel> ModuleServiceQueryModels { get; set; }
    }
}
