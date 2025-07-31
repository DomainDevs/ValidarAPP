// -----------------------------------------------------------------------
// <copyright file="ParametersServiceModel.cs" company="SISTRAN">
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
    /// Contiene la propiedad de lista de parametros
    /// </summary>
    [DataContract]
    public class ParametersServiceModel : ErrorServiceModel
    {

        /// <summary>
        /// Obtiene o establece una lista de Parametros
        /// </summary>
        [DataMember]
        public List<ParameterServiceModel> ParameterServiceModel { get; set; }

    }
}
