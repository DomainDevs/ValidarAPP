// -----------------------------------------------------------------------
// <copyright file="ModuleSubmoduleServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ModuleSubmoduleServiceQueryModel" />
    /// </summary>
    [DataContract]
    public class ModuleSubmoduleServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Lista subModulos
        /// </summary>
        [DataMember]
        public List<SubModuleServicesQueryModel> SubModuleQueryModel { get; set; }
    }
}
