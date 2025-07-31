using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    
  
    public class TextPrecatalogued
    {
        public int ConditionTextId { get; set; }
        public string TextTitle { get; set; }
        public string TextBody { get; set; }
        public int ConditionLevelCode { get; set; }
        public int ConditionTextIdCod { get; set; }
        public int CondTextLevelId { get; set; }
        public int? ConditionLevelId { get; set; }
        public int IsAutomatic { get; set; }
        public string DescriptionLevel { get; set; }
        public string DescriptionBranch { get; set; }
        public string DescriptionRiskCoverange { get; set; }
        public string DescriptionCoverange { get; set; }
        public int? CommercialBranch { get; set; }
        public int? CoveredRisk { get; set; }
        public int? Coverage { get; set; }
    }
}