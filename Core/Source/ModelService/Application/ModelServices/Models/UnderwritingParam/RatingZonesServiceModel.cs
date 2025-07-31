// -----------------------------------------------------------------------
// <copyright file="RatingZonesServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo zonas de tarifacion
    /// </summary>
    [DataContract]
    public class RatingZonesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece las zonas de tarifacion
        /// </summary>
        [DataMember]
        public List<RatingZoneServiceModel> RatingZones { get; set; }
    }
}