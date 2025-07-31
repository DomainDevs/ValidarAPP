using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class CoverageModelsView
    {
        /// <summary>
        /// Id Cobertura
        /// </summary>     
        [Display(Name = "LabelCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlCoverage")]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Cobertura
        /// </summary>     
        public string Description { get; set; }

        /// <summary>
        /// Id Tipo Riesgo
        /// </summary>
        [Display(Name = "LabelRiskType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int RiskTypeId { get; set; }

        /// <summary>
        /// Id Grupo de Cobertura
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int CoverageGroupId { get; set; }


        /// <summary>
        /// Id del objeto del seguro
        /// </summary>     
        [Display(Name = "LabelInsuranceObject", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCoverage")]
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// Obligatorio
        /// </summary>    
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Objeto inicialmente incluido
        /// </summary>        
        public bool IsSelected { get; set; }

        /// <summary>
        /// Sublimite cobertura
        /// </summary>
        [Display(Name = "LabelPercentageSublimit", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public decimal SubLimitAmount { get; set; }

        ///// <summary>
        ///// Lista Cobeturas aliadas
        ///// </summary>
        //public List<CoverageModelsView> coverages { get; set; }

        
    }
}