using Sistran.Company.Application.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
        public StateViewModel State { get; set; }
        
        public CountryViewModel  Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldCityDesc")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldCitySmall")]
        public string SmallDescription { get; set; }

        public ErrorDTO ErrorDTO { get; set; }
    }

    public class StateViewModel
    {
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class CountryViewModel
    {
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldCountries")]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}