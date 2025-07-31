using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class CoverageValueViewModel
    {
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldPorcentaje")]
        [RegularExpression(@"(^(100(?:\,0{1,2})?))|(?!^0*$)(?!^0*\,0*$)^\d{1,2}(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorInvalidFormat")]
        public decimal? Porcentage { get; set; }

        public CoverageView Coverage { get; set; }
        public PrefixView Prefix { get; set; }

    }

    public class CoverageView
    {
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldCoverage")]
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class PrefixView
    {
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Errorfieldprefix")]
        public  int Id { get; set; }
        public string Description { get; set; }
    }
}