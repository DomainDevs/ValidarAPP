using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class GuaranteeListViewModel
    {
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        public string Description { get; set; }

        [Display(Name = "LabelNominalValue", ResourceType = typeof(App_GlobalResources.Language))]
        public string NominalValue { get; set; }

        [Display(Name = "LabelHeaderDocumentNumber", ResourceType = typeof(App_GlobalResources.Language))]
        public string HeaderDocumentNumber { get; set; }

        [Display(Name = "LabelClosedState", ResourceType = typeof(App_GlobalResources.Language))]
        public string IsClosed { get; set; }

        [Display(Name = "LabelStatus", ResourceType = typeof(App_GlobalResources.Language))]
        public string Status { get; set; }

    }
}