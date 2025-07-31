using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class ParameterModel
    {
        [Display(ResourceType = typeof(Global), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Module")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Order")]
        public int Order { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string ParameterDescription { get; set; }

        [Display(ResourceType = typeof(Global), Name = "DataTypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "DataType")]
        public string TypeDescription { get; set; }
    }
}