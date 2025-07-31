// -----------------------------------------------------------------------
// <copyright file="RiskTypesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo lista tipo de riesgo
    /// </summary>
    [DataContract]
    public class RiskTypesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista tipo de riesgo
        /// </summary>
        [DataMember]
        public List<RiskTypeServiceModel> RiskTypeServiceModels { get; set; }
    }
}
