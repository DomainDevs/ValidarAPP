// -----------------------------------------------------------------------
// <copyright file="BasicPersonsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>David S. Niño T.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades la información basica de personas
    /// </summary>
    [DataContract]
    public class BasicPersonsServiceModel:ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista la informacion basicas de personas
        /// </summary>
        [DataMember]
        public List<BasicPersonServiceModel> BasicPersonServiceModel { get; set; }
    }
}
