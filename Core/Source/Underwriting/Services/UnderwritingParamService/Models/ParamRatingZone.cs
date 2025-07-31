// -----------------------------------------------------------------------
// <copyright file="ParamRatingZone.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using UnderwritingServices.Models;
    /// <summary>
    /// Modelo zona de tarifacion
    /// </summary>
    public class ParamRatingZone : RatingZone
    {
        /// <summary>
        /// Obtiene o establece un valor que indica si es predeterminado
        /// </summary>
        public bool IsDefault { get; set; }
    }
}