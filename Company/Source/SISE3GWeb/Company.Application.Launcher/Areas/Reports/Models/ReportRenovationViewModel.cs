using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reports.ReportRenovation.Models
{
    public class ReportRenovationViewModel
    {
        public int Prefix { get; set; }

        public int? Branch { get; set; }

        public int? SalePoint { get; set; }

        public int? IntermediarylId { get; set; }

        public int? DirectorId { get; set; }

        public bool? Renewed { get; set; }

        public DateTime SinceDate { get; set; }

        public DateTime UntilDate { get; set; }


    }
}