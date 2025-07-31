using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models.Reports
{
    [KnownType("ProcessMassiveDetailsReport")]
    public class ProcessMassiveDetailsReport
    {
        //Cabecera
        public int TempReinsuranceProcessId { get; set; }
        public string BranchDescription { get; set; }
        public string PrefixDescription { get; set; }
        public string PolicyNumber { get; set; }
        public int RiskNumber { get; set; }
        public int CoverageNumber { get; set; }
        public int EndorsementNumber { get; set; }
        public string ErrorDescription { get; set; }
    }
}