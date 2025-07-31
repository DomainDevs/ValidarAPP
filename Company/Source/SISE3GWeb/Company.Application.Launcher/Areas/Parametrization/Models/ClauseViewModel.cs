// -----------------------------------------------------------------------
// <copyright file="ClauseViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Modelo para la vista
    /// </summary>
    public class ClauseViewModel
    {
        /// <summary>
        /// Obtiene o establece Id clausula
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece nombre de la clausula 
        /// </summary>
        [StringLength(10, ErrorMessage = "Maximo 50 Caracteres")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece titulo de la clausulas
        /// </summary>
        [StringLength(60, ErrorMessage = "Maximo 60 Caracteres")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }

        /// <summary>
        /// Obtiene o establece nivel
        /// </summary>
        [Display(Name = "LabelLevels", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public int Level { get; set; }

        /// <summary>
        /// Obtiene o establece valor nivel de descripcion
        /// </summary>
        public string LevelDescription { get; set; }

        /// <summary>
        /// Obtiene o establece ramo comercial
        /// </summary>
        [Display(Name = "LabelCommercialBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public int? CommercialBranch { get; set; }

        /// <summary>
        /// Obtiene o establece  cobertura
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public int? Coverage { get; set; }

        /// <summary>
        /// Obtiene o establece nombre de la cobertura
        /// </summary>
        public string CoverageName { get; set; }

        /// <summary>
        /// Obtiene o establece nombre del tipo de riesgo
        /// </summary>
        public string RiskTypeName { get; set; }

        /// <summary>
        /// Obtiene o establece nombre ramo comercial
        /// </summary>
        public string CommercialBranchName { get; set; }


        /// <summary>
        /// Obtiene o establece nombre del ramo tecnico
        /// </summary>
        public string LineBusinessName { get; set; }

        /// <summary>
        /// Obtiene tipo de riesgo cubierto
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public int? CoveredRisk { get; set; }


        /// <summary>
        /// Obtiene tipo de riesgo cubierto
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public int? LineBusiness { get; set; }

        /// <summary>
        /// Obtiene o establece objeto del seguro
        /// </summary>
        public string ObjectInsurance { get; set; }

        /// <summary>
        /// Obtiene o establece un valor de amparo
        /// </summary>
        public string Protection { get; set; }

        /// <summary>
        /// Obtiene o establece un valor de fecha de inicio de entrada
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public DateTime InputStartDate { get; set; }

        /// <summary>
        /// Obtiene o establece fecha vencimiento
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Obtiene o establece un valor requerido
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Obtiene texto precatalogado
        /// </summary>
        [StringLength(60, ErrorMessage = "Maximo 60 Caracteres")]
        public string PrecatalogedText { get; set; }

        /// <summary>
        /// Obtiene texto de la clausula
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public string ClauseText { get; set; }

        /// <summary>
        /// Obtiene o establece id nivel de la clausula
        /// </summary>
        public int ClauseLevelId { get; set; }

        /// <summary>
        /// Obtiene o establece estado
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}
