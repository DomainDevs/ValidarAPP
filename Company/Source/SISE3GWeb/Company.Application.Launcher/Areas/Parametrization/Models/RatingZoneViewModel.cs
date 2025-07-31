// -----------------------------------------------------------------------
// <copyright file="RatingZoneViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using App_GlobalResources;
    using Application.ModelServices.Models.UnderwritingParam;
    using MODCO = Application.ModelServices.Models.CommonParam;

    /// <summary>
    /// model view zonas de tarifacion
    /// </summary>
    public class RatingZoneViewModel 
    {
        /// <summary>
        /// Obtiene o establece el identificador
        /// </summary>
        [Display(ResourceType = typeof(Language), Name = "LabelRatingZoneCode")]
        public int RatingZoneCode { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo
        /// </summary>
        [Display(ResourceType = typeof(Language), Name = "LabelPrefix")]
        [Required(ErrorMessageResourceType = typeof(Language), ErrorMessageResourceName = "FieldRequired")]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del ramo 
        /// </summary>
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [Display(ResourceType = typeof(Language), Name = "LabelDescription")]
        [Required(ErrorMessageResourceType = typeof(Language), ErrorMessageResourceName = "FieldRequired")]
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [Display(ResourceType = typeof(Language), Name = "LabelDescriptionShort")]
        [Required(ErrorMessageResourceType = typeof(Language), ErrorMessageResourceName = "FieldRequired")]
        [MaxLength(15)]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es predefinido
        /// </summary>
        [Display(ResourceType = typeof(Language), Name = "LabelDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de ciudades
        /// </summary>
        public List<MODCO.CityServiceRelationModel> Cities { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public int StatusTypeService { get; set; }
    }
}