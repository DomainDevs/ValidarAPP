// -----------------------------------------------------------------------
// <copyright file="RiskTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo tipo de riesgo
    /// </summary>
    [DataContract]
    public class RiskTypeServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Codigo del tipo de riesgo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion del tipo de riesgo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
