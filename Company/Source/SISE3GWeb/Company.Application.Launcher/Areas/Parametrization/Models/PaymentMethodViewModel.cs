using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sistran.Core.Application.EntityServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class PaymentMethodViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "MaxLengthPaymentMethodDesc")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelTinyDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string TinyDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelSmallDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(30, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "MaxLengthPaymentMethodDesc")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public PaymentMethodTypeViewModel PaymentMethodType { get; set; }


        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}