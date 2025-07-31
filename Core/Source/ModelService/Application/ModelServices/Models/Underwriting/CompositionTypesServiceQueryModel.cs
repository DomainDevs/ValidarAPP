// -----------------------------------------------------------------------
// <copyright file="CompositionTypesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de nivel de influencia
    /// </summary>
    [DataContract]
    public class CompositionTypesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado de los niveles de influencia
        /// </summary>
        [DataMember]
        public List<CompositionTypeServiceQueryModel> CompositionTypeServiceQueryModels { get; set; }
    }
}
