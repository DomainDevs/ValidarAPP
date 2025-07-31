
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class TechnicalBranchProtectionViewModel
    {
        public int Id { get; set; }
        public List<ProtectionViewModel> PerilNotAssign { get; set; }
        public List<ProtectionViewModel> PerilAssign { get; set; }
    }
}