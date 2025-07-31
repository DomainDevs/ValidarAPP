using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sistran.Core.Application.ReinsuranceServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ModificationReinsuranceContractModel : ReinsuranceAllocationDTO
    {
        internal int ReinsuranceAllocationId;

        public string Sum { get; set; }

        public string PremiumAllocation { get; set; }
    }
}