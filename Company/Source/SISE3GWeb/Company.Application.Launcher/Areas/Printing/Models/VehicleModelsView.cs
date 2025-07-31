using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Printing.Models
{
    public class VehicleModelsView
    {
        [Display(Name = "LabelPrinterHeaderDetail", ResourceType = typeof(App_GlobalResources.Language))]
        public int VehicleReportType { get; set; }

        [Display(Name = "LabelDetailRiskPrinter", ResourceType = typeof(App_GlobalResources.Language))]
        public bool AttachRisksDetail { get; set; }

        [Display(Name = "LabelLicencePlatePrinter", ResourceType = typeof(App_GlobalResources.Language))]
        public string LicensePlates { get; set; }

        [Display(Name = "LabelRankRisk", ResourceType = typeof(App_GlobalResources.Language))]
        public int? RangeFrom { get; set; }

        [Display(Name = "LabelRankRisk", ResourceType = typeof(App_GlobalResources.Language))]
        public int? RangeTo { get; set; }
    }
}