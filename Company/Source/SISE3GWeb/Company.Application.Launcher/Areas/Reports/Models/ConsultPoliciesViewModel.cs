using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reports.Models
{
    public class ConsultPoliciesViewModel
    {
        [Display(Name = "LabelDateStart", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime labelInitialDateOfSelection { get; set; }

        [Display(Name = "DateUntil", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime labelFinalyDateOfSelection { get; set; }


    }
}