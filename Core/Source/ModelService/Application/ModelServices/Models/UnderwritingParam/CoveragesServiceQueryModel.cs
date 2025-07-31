// -----------------------------------------------------------------------
// <copyright file="CoveragesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Modelo de servicio de consulta de Coberturas.
    /// </summary>
    [DataContract]
    public class CoveragesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Propiedad de la Lista de Coberturas.
        /// </summary>
        [DataMember]
        public List<CoverageServiceQueryModel> CoverageServiceQueryModel { get; set; }
    }
}
