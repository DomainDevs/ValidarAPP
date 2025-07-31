using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models.Reports
{
    [KnownType("ReinsuranceLayerReport")]
    public class ReinsuranceLayerReport
    {
        //Cabecera
        public int IssueLayerId { get; set; }
        public int ReinsuranceNumber { get; set; }
        public string Description { get; set; }
        public string ProcessDate { get; set; }
        public int LayerNumber { get; set; }
        /*----------------------------------------------*/
        public decimal LayerPercentage { get; set; }
        public decimal PremiumPercentage { get; set; }
    }
}