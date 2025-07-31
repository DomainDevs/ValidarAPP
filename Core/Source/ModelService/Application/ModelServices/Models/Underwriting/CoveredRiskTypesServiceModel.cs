// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de consulta de los tipos de riesgo cubierto.
    /// </summary>
    [DataContract]
    public class CoveredRiskTypesServiceModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// Propiedad de la Lista de tipos de riesgo cubierto.
        /// </summary>
        [DataMember]
        public List<CoveredRiskTypeServiceModel> CoveredRiskTypeServiceModel { get; set; }
    }
}
