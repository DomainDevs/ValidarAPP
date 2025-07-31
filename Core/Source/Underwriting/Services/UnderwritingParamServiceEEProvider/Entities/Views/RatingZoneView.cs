// -----------------------------------------------------------------------
// <copyright file="RatingZoneView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Framework.DAF;
    using Framework.Views;

    /// <summary>
    /// Vista de zonas de tarifacion y ciudades
    /// </summary>
    [Serializable]
    public class RatingZoneView : BusinessView
    {
        /// <summary>
        /// Obtiene colección zonas de tarifacion
        /// </summary>
        public BusinessCollection RatingZone => this["RatingZone"];

        /// <summary>
        ///  Obtiene colección ramos
        /// </summary>
        public BusinessCollection Prefix => this["Prefix"];

        /// <summary>
        /// Obtiene colección zonas de tarifacion
        /// </summary>
        public BusinessCollection CountryRatingZone => this["CountryRatingZone"];

        /// <summary>
        /// Obtiene colección ciudades
        /// </summary>
        public BusinessCollection City => this["City"];

        /// <summary>
        /// Obtiene colección estados
        /// </summary>
        public BusinessCollection State => this["State"];

        /// <summary>
        /// Obtiene colección paises
        /// </summary>
        public BusinessCollection Country => this["Country"];
    }
}
