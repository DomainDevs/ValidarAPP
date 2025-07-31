// -----------------------------------------------------------------------
// <copyright file="ParamRatingZoneCity.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using CommonService.Models;
    using System.Collections.Generic;
    /// <summary>
    /// Modelo Zonas de tarifacion ciudades
    /// </summary>
    public class ParamRatingZoneCity
    {
        /// <summary>
        /// Obtiene o establece la zona de tarifacion
        /// </summary>
        public ParamRatingZone RatingZone { get; set; }

        /// <summary>
        /// Obtiene o establece las ciudades
        /// </summary>
        public List<City> Cities { get; set; }
    }
}