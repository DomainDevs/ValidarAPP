using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class DataTypeModel
    {
        [Display(ResourceType = typeof(Global), Name = "DataTypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "DataType")]
        public string TypeDescription { get; set; }
    }
}