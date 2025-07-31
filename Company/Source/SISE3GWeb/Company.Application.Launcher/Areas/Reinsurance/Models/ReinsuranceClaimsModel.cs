using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ReinsuranceClaimsModel 
    {
        public string Variance { get; set; }
        
        public string NewAmount { get; set; }

        public string SourceAmount  { get; set; }

        public int TempClaimReinsSourceId { get; set; }
    }
}