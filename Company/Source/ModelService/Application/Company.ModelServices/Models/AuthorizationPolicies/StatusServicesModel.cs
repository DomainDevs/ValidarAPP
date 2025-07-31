// -----------------------------------------------------------------------
// <copyright file="HierarchyServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies
{
    using Sistran.Company.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Defines the <see cref="StatusServicesModel" />
    /// </summary>


    public class StatusServicesModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de Modulos
        /// </summary>
        [DataMember]
        public List<StatusServicemodel> StatusServicemodel { get; set; }
    }
}
