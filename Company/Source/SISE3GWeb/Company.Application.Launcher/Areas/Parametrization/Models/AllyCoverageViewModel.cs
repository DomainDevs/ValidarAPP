using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Clase View Model de Cobertura aliada
    /// </summary>
    /// <author>Germán F. Grimaldi</author>
    /// <date>14/08/2018</date>
    /// <purpose>Representa el modelo de vista de Cobertura aliada</purpose>
    public class AllyCoverageViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura Aliada
        /// </summary>
        public int AllyCoverageId { get; set; }

        /// <summary>
        /// Obtiene o establece Id de Cobertura Aliada
        /// </summary>
        public CoverageQueryViewModel AllyCoverageId_object { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        public int CoverageId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        public CoverageQueryViewModel CoverageId_object { get; set; }

        /// <summary>
        /// Obtiene o establece el id del amparo relacionada a la cobertura
        /// </summary>
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileStringHundred")]
        public decimal CoveragePct { get; set; }

        //internal static AllyCoverageViewModel CreateAllyCoverageModelView(AllyCoverage ally)
        //{
        //    AllyCoverageViewModel model = new AllyCoverageViewModel();
        //    model.AllyCoverageId = ally.;
        //    model.CoverageId = group.IdGroupPolicies;
        //    model.CoveragePct = group.Module;
        //    model.Key = group.Key;

        //    return model;
        //}

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService Status { get; set; }

    }
    public class CoverageQueryViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id del amparo relacionada a la cobertura
        /// </summary>
        public int PerilId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del subramo tecnico relacionado a la cobertura
        /// </summary>
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo tecnico relacionado a la cobertura
        /// </summary>
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del objeto del seguro relacionado a la cobertura
        /// </summary>
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de cobertura
        /// </summary>
        [Display(Name = "NameCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        public string PrintDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Obtiene la fecha de expiración de la cobertura
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Obtiene o establece el id del nivel del influencia
        /// </summary>
        public int? CompositionTypeId { get; set; }

        /// <summary>
        /// Obtiene o estableec el id de reglas
        /// </summary>
        public int? RuleSetId { get; set; }
    }
}