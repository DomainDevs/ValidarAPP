// -----------------------------------------------------------------------
// <copyright file="ErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies
{
    using Sistran.Company.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo general
    /// </summary>
    [DataContract]
    public class RejectionCausesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece listado
        /// </summary>
        [DataMember]
        public List<RejectionCauseServiceModel> RejectionCauseServiceModel { get; set; }
    }
}
