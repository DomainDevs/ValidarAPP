// -----------------------------------------------------------------------
// <copyright file="CitiesServiceRelationModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using Param;

    /// <summary>
    /// Modelo ciudades
    /// </summary>
    public class CitiesServiceRelationModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de ciudades
        /// </summary>
        public List<CityServiceRelationModel> Cities { get; set; }
    }
}