using MOS = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductFormModelView
    {
        public string StrCurrentFrom { get; set; }
        public string FormNumber { get; set; }
        public MOS.GroupCoverage GroupCoverage { get; set; }
    }
}