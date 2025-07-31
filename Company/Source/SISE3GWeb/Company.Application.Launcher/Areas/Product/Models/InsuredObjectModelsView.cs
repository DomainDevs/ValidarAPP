using Sistran.Company.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class InsuredObjectModelsView
    {

        /// <summary>
        /// Id del objeto del seguro
        /// </summary>     
        [Display(Name = "LabelInsuranceObject", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlInsuranceObject")]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del Objecto
        /// </summary>     
        public string Description { get; set; }

        /// <summary>
        /// Id Grupo de Cobertura
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int CoverageGroupId { get; set; }

        /// <summary>
        /// Id Tipo Riesgo
        /// </summary>
        [Display(Name = "LabelRiskType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int RiskTypeId { get; set; }

        /// <summary>
        /// Objeto inicialmente incluido
        /// </summary>        
        public bool IsSelected { get; set; }

        /// <summary>
        /// Obligatorio
        /// </summary>        
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProductCoveragesModelsView> Coverages { get; set; }
    }
}