// -----------------------------------------------------------------------
// <copyright file="ClauseServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de lista de Clausulas
    /// </summary>
    [DataContract]
    public class ClausesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado de las clausulas
        /// </summary>
        [DataMember]
        public List<ClauseServiceQueryModel> ClauseServiceModels { get; set; }
    }
}
