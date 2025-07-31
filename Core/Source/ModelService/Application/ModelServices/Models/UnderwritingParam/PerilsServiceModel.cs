// -----------------------------------------------------------------------
// <copyright file="PerilsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo amparos
    /// </summary>
    [DataContract]
    public class PerilsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los amparos (Modelo del servicio)
        /// </summary>
        [DataMember]
        public List<PerilServiceModel> PerilServiceModels { get; set; }
    }
}
