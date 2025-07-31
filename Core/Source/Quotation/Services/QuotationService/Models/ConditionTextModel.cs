using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using  Sistran.Core.Application.ModelServices.Models.Underwriting;

namespace Sistran.Core.Application.QuotationServices.Models
{
    [DataContract]
    public class ConditionTextModel : CondTextLevelModel
    {
        [DataMember]
        public int ConditionTextId { get; set; }
        [DataMember]
        public string TextTitle { get; set; }
        [DataMember]
        public string TextBody { get; set; }
        [DataMember]
        public int ConditionLevelCode { get; set; }
        [DataMember]
        public CondTextLevelModel CondTextLevelModel
        {
            get; set;
        }
        [DataMember]
        public Prefix Prefix
        {
            get; set;
        }
        [DataMember]
        public Coverage coverage
        {
            get; set;
        }
        [DataMember]
        public RiskTypeServiceModel RiskTyprCoverage
        {
            get;set;
        }
        [DataMember]
        public ConditionLevel ConditionLevel
        {
            get;set;
        }
    }
}
