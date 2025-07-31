using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class CopyProductModelsView
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [Display(Name = "LabelProduct", ResourceType = typeof(App_GlobalResources.Language))]
        public int Id { get; set; }
        
        /// <summary>
        /// descripcion larga del nuevo producto 
        /// </summary>
        [Display(Name = "LabelDescriptionProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// descripcion corta del nuevo producto
        /// </summary>
        [Display(Name = "LabelDescriptionProductReduced", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionReduced { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los grupos de coberturas
        /// </summary>
        public bool CopyGroupCoverages { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los planes de pago
        /// </summary>
        public bool CopyPaymentPlan { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los paquetes de reglas
        /// </summary>
        public bool CopyRuleSet { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia las formas de impresion
        /// </summary>
        public bool CopyPrintingFormes { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los intermediarios
        /// </summary>
        public bool CopyAgent { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los Limites RC
        /// </summary>
        public bool CopyLimitRC { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los Guiones
        /// </summary>
        public bool CopyScript { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia las actividades del riesgo
        /// </summary>
        public bool CopyActivityRisk { get; set; }
        
    }
}
