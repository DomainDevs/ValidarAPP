// -----------------------------------------------------------------------
// <copyright file="ClausesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------   ------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo lista de clausulas
    /// </summary>
    [DataContract]
    public class ClausesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de clausula
        /// </summary>
        [DataMember]
        public List<ClauseServiceModel> ClauseServiceModels { get; set; }
    }
}
