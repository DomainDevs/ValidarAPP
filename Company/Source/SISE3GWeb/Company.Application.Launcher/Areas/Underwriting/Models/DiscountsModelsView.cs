using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
//using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class DiscountsModelsView
    {
        public int Id { get; set; }
        public string Description { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "DiscountsMessageError")]
        public string DiscountsParametrization { get; set;}
        public int TaxeType { get; set; }
        public string TaxeTypeDescription { get; set; }
        public string Taxe { get; set; }
        public decimal totalDiscounts { get; set; }
        
    }
}