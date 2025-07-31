// -----------------------------------------------------------------------
// <copyright file="SearchRuleModelView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.RulesScriptsServices.Enums;
    using MRules = Application.RulesScriptsServices.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Busqueda avanzada de regla 
    /// </summary>
    public class AdvSearchRuleModelView
    {
        /// <summary>
        /// Ide de paquete de reglas
        /// </summary>
        public int IdAdv { get; set; }
        
        /// <summary>
        /// Descripción de paquete de reglas
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public string RuleDescription{ get; set; }
        
        /// <summary>
        /// Fecha de creación 
        /// </summary>
        [Display(Name = "DateCreate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? DateCreation { get; set; }
        
        /// <summary>
        /// Fecha de modificación
        /// </summary>
        [Display(Name = "DateModifid", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? DateModification { get; set; }
        
        /// <summary>
        /// Id tipo de paquete de reglas
        /// </summary>
        [Display(Name = "LabelPackages", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int TypePackageId { get; set; }
        
        /// <summary>
        /// Id de nivel
        /// </summary>
        [Display(Name = "LabelLevels", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int LevelIdAdv { get; set; }

    }
}