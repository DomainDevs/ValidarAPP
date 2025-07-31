// -----------------------------------------------------------------------
// <copyright file="InfringementsLogServiceModel.cs" company="SISTRAN">
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
    /// Contiene la propiedad de lista dias de infraccion
    /// </summary>
    [DataContract]
    public class InfringementsLogServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista dias de infraccion
        /// </summary>
        [DataMember]
        public List<InfringementLogServiceModel> InfringementLogServiceModel { get; set; }

    }
}
