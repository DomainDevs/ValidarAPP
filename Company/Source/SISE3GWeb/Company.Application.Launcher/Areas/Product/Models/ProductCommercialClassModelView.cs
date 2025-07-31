namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductCommercialClassModelView
    {
        public bool DefaultRiskCommercial { set; get; }
        public RiskCommercialClassModelView RiskCommercialClass { set; get; }
    }

    public class RiskCommercialClassModelView
    {
        public int RiskCommercialClassCode { get; set; }
    }
}