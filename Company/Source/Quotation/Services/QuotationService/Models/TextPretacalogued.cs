using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.QuotationServices.Models
{
    [DataContract]
    public class TextPretacalogued: ParametricServiceModel
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
        public int ConditionTextIdCod { get; set; }
        [DataMember]
        public int CondTextLevelId { get; set; }
        [DataMember]
        public int? ConditionLevelId { get; set; }
        [DataMember]
        public bool IsAutomatic { get; set; }
        [DataMember]
        public string DescriptionLevel { get; set; }
        [DataMember]
        public string DescriptionBranch { get; set; }
        [DataMember]
        public string DescriptionRiskCoverange { get; set; }
        [DataMember]
        public string DescriptionCoverange { get; set; }

    }
}
