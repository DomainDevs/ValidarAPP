using System;
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class RequestGroupingViewModel
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        public int Branch { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelRequestGroupingDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(120, MinimumLength = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Tomador
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelHolder", ResourceType = typeof(App_GlobalResources.Language))]
        public string PolicyHolder { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]       
        public string PrincipalAgent { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]       
        public int Prefix { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelCommercialProduct", ResourceType = typeof(App_GlobalResources.Language))]
        public int Product { get; set; }

        /// <summary>
        /// Tipo de póliza
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelPolicyType", ResourceType = typeof(App_GlobalResources.Language))]
        public int PolicyType { get; set; }

        /// <summary>
        /// Plan de pago
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelPaymentPlan", ResourceType = typeof(App_GlobalResources.Language))]
        public int PaymentShedule { get; set; }

        /// <summary>
        /// Si es vigencia abierta
        /// </summary>
        public bool IsOpenEffect { get; set; }

        /// <summary>
        /// V. Desde
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelFrom", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// V. Hasta
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelTo", ResourceType = typeof(App_GlobalResources.Language))]        
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Display(Name = "LabelObservations", ResourceType = typeof(App_GlobalResources.Language))]     
        public string Annotations { get; set; }

        /// <summary>
        /// Agencia del intermediario
        /// </summary>
        /// 
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]     
        public int AgencyId { get; set; }
    

        /// <summary>
        /// DescriptionRequest
        /// </summary>
        [StringLength(120)]
        public string DescriptionRequest { get; set; }

        /// <summary>
        /// Descripcion grupo facturación
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelBillingGroup", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60)]
        public string DescriptionBillingGroup { get; set; }
    }
}