using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class LineModel
    {
        public int LineId { get; set; }

        [Required]
        public int CumulusTypeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The field {0} should have minimum {2} and maximum {1} characters", MinimumLength = 10)]
        public string Description { get; set; }
        
    }
}