// -----------------------------------------------------------------------
// <copyright file="RatingZoneServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using CommonParam;
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Underwriting;

    /// <summary>
    /// Modelo de zona de tarifacion
    /// </summary>
    [DataContract]
    public class RatingZoneServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el codigo de la zona de tarifacion
        /// </summary>
        [DataMember]
        public int RatingZoneCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es predeterminado
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Obtiene o establece el ramo comercial
        /// </summary>
        [DataMember]
        public PrefixServiceQueryModel Prefix { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de ciudades
        /// </summary>
        [DataMember]
        public List<CityServiceRelationModel> Cities { get; set; }
    }
}