// -----------------------------------------------------------------------
// <copyright file="BasicCompanysServiceModel.cs" company="SISTRAN">
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
    /// Contiene las propiedades de tipos de Sucursal.
    /// </summary>
    [DataContract]
    public class BasicCompanysServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista de Informacion basica de compañias
        /// </summary>
        [DataMember]
        public List<BasicCompanyServiceModel> BasicCompanyServiceModel { get; set; }
    }
}
