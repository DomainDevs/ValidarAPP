using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Collective.Models
{
    public class EndorsementModelView
    {
        /// <summary>
        /// Id Sucursal
        /// </summary>
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BranchId { get; set; }

        /// <summary>
        /// Descripcion Sucursal
        /// </summary>
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Branch { get; set; }


        /// <summary>
        /// Id Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixId { get; set; }

        /// <summary>
        /// Descripcion Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Prefix { get; set; }

        /// <summary>
        /// Id Producto
        /// </summary>
        [Display(Name = "LabelCommercialProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ProductId { get; set; }


        /// <summary>
        /// Descripcion Producto
        /// </summary>
        [Display(Name = "LabelCommercialProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Product { get; set; }

        /// <summary>
        /// Número de Poliza
        /// </summary>
        [Display(Name = "LabelNumberPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(12)]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>
        [Display(Name = "LabelEndorsement", ResourceType = typeof(App_GlobalResources.Language))]
        public int? EndorsementId { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>        
        public int? PolicyId { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [Display(Name = "LabelPlate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(15)]
        public string LicensePlate { get; set; }

        public decimal AmountInsured { get; set; }
        public decimal Expenses { get; set; }
        public decimal FullPremium { get; set; }
        public decimal Premium { get; set; }
        public decimal RiskCount { get; set; }
        public decimal Taxes { get; set; }

        /// <summary>
        /// IdTypeLoad
        /// </summary>
        [Required]
        public int LoadTypeId { get; set; }

        /// <summary>
        /// Tipo de endoso
        /// </summary>
        public int EndorsementType { get; set; }
    }
}