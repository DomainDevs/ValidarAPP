// -----------------------------------------------------------------------
// <copyright file="CountiesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using Param;

    /// <summary>
    /// Modelo de paises
    /// </summary>
    public class CountriesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de paises
        /// </summary>
        public List<CountryServiceQueryModel> Counties { get; set; }
    }
}