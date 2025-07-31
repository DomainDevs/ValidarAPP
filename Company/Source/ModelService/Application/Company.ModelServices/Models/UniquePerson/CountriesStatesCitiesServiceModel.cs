// -----------------------------------------------------------------------
// <copyright file="CountriesStatesCitiesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;    

    /// <summary>
    /// Contiene las propiedades de lo país, ciudad, estado
    /// </summary>
    [DataContract]
    public class CountriesStatesCitiesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista de lo país, ciudad, estado.
        /// </summary>
        [DataMember]
        public List<CountryStateCityServiceModel> CountryStateCityServiceModel { get; set; }
    }
}