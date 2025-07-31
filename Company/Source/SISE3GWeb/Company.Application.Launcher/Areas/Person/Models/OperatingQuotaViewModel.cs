using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class OperatingQuotaViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int LineBusinessCd { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int OperatingQuotaCd { get; set; }
        /// <summary>
        /// Moneda
        /// </summary>
        [Display(Name = "LabelCoin", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [Required]
        public int Currency { get; set; }


        /// <summary>
        /// Cupo Operativo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelValuationValue", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal OperatingQuotaAmt { get; set; }

        /// <summary>
        /// Fecha avalúo
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "LabelValuationDate", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        public DateTime? CurrentTo { get; set; }
    }
}