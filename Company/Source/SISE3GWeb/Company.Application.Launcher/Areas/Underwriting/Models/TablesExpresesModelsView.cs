using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class TablesExpresesModelsView
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "TablesExpresesMessageError")]
        public string parameterizedExpenses { get; set; }

        public string ExecuteType { get; set; }

        public int TaxesTypeId { get; set; }

        public string TaxesType { get; set; }

        public double Taxes { get; set; }

        public int? RuleId { get; set; }

        public string RuleDescription { get; set; }

        public bool IsObligatory { get; set; }

        public bool IsInitiallyIncluded { get; set; }

        public string TotalTablesExpreses { get; set; }

    }
}