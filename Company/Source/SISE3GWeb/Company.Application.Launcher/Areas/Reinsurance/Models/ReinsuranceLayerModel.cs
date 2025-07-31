using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Sistran.Core.Application.ReinsuranceServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ReinsuranceLayerModel : ReinsuranceLayerIssuanceDTO
    {
        
        [Required]
        public string SumPercentage { get; set; }
        
        [Required]
        public new string PremiumPercentage { get; set; }

        public int ReinsLayerId { get; set; }
        
        public int ReinsSourceId { get; set; }

        public int ReinsLayerNumber { get; set; }

        public int TempReinsuranceProcessId { get; set; }
    }
}