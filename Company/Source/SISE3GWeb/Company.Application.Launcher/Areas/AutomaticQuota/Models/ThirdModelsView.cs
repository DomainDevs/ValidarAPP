using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    
    public class ThirdModelsView
    {
        public  int id { get; set; }

        public RiskCenterModelsView riskCenter{ get; set; }
        
        public RestrictiveModelsView restrictive{ get; set; }
        
        public PromissoryNoteSignatureModelsView promissoryNoteSignature{ get; set; }
        
        public ReportListSisconcModelsView reportListSisconc { get; set; }
        
        public DateTime CifinQuery { get; set; }
        
        public decimal PrincipalDebtor { get; set; }
        
        public decimal Cosigner { get; set; }
        
        public decimal Total { get; set; }

    }
}
