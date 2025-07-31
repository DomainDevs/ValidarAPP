// -----------------------------------------------------------------------
// <copyright file="AllyCoveragesServiceModel.cs" company="SISTRAN">
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
    /// Modelo lista de Coberturas Aliada
    /// </summary>
    [DataContract]
    public class AllyCoveragesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de coberturas aliadas
        /// </summary>
        [DataMember]
        public List<AllyCoverageServiceModel> AllyCoverageServiceModels { get; set; }
    }
}
