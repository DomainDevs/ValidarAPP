// -----------------------------------------------------------------------
// <copyright file="DiscontinuitysLogServiceModelErrorServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Common
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Contiene la propiedad de lista dias Discontinuidad
    /// </summary>
    [DataContract]
    public class DiscontinuitysLogServiceModel:ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista dias Discontinuidad
        /// </summary>
        [DataMember]
        public List<DiscontinuityLogServiceModel> DiscontinuityLogServiceModel { get; set; }
    }
}
