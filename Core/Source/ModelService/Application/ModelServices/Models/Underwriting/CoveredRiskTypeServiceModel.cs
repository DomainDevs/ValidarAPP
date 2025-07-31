// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    /// <summary>
    /// Modelo de servicio de los tipos de riesgo cubierto.
    /// </summary>
    [DataContract]
    public class CoveredRiskTypeServiceModel
    {
        /// <summary>
        /// Id del tipo de riesgo cubierto.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción del tipo de riesgo cubierto.
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
