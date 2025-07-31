using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Application.EntityServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class VehicleMakeViewModel 
    {

        public int id { get; set; }

        public string description { get; set; }

        public StatusTypeService StatusTypeService { get; set; }

    }
}