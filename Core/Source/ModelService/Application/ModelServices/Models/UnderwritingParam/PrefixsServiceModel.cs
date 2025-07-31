// -----------------------------------------------------------------------
// <copyright file="PrefixsServiceModel.cs" company="SISTRAN">
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
    /// Modelo para lista de ramo comercial
    /// </summary>
    [DataContract]
    public class PrefixsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de ramo comercial
        /// </summary>
        [DataMember]
        public List<PrefixServiceModel> PrefixServiceModels { get; set; }
    }
}
