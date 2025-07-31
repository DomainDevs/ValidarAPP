// -----------------------------------------------------------------------
// <copyright file="CoverageViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// ModelView de cobertura
    /// </summary>
    public class CoverageViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de cobertura
        /// </summary>
        [Display(Name = "NameCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]        
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Obtiene o establece el id del amparo relacionada a la cobertura
        /// </summary>
        public int PerilId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del objeto del seguro relacionado a la cobertura
        /// </summary>
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo tecnico relacionado a la cobertura
        /// </summary>
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del subramo tecnico relacionado a la cobertura
        /// </summary>
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del nivel del influencia
        /// </summary>
        public int? CompositionTypeId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es de asistencia
        /// </summary>
        public bool IsAssistance { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si  la cobertura es de prima minima
        /// </summary>
        public bool IsAccMinPremium { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es de impresion
        /// </summary>
        public bool IsImpression { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es de seriedad de la oferta
        /// </summary>
        public bool IsSeriousOffer { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de impresion de la cobertura
        /// </summary>
        [Display(Name = "CoverageNamePrint", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        public string ImpressionName { get; set; }

        /// <summary>
        /// Obtiene o establece el valor asegurado a imprimir de la cobertura
        /// </summary>
        [Display(Name = "InsuredValuePrint", ResourceType = typeof(App_GlobalResources.Language))]        
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        public string ImpressionValue { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService Status { get; set; }

        /// <summary>
        /// Obtiene o establece las clausulas 
        /// </summary>
        public List<PartialsInformationViewModel> Clauses { get; set; }

        /// <summary>
        /// Obtiene o establece los deducibles
        /// </summary>
        public List<PartialsInformationViewModel> Deductibles { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de detalle
        /// </summary>
        public List<PartialsInformationViewModel> DetailTypes { get; set; }

        /// <summary>
        /// Obtiene o establece la homologacion 2g de la cobertura
        /// </summary>
        public CoverageHomologation2GViewModel Homologation2G { get; set; }

        /// <summary>
        /// Obtiene o establece la homologacion 2g de la cobertura
        /// </summary>
        public List<CoCoverageViewModel> CoCoverageServiceModels { get; set; }
    }
}