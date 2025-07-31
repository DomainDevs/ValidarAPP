
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class BankFieldFormatModels
    {
        public int AccountBankId { get; set; }
        public int FormatId { get; set; }
        public int FormatTypeId { get; set; }
        public int OrderNumber { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public string Description { get; set; }
        public string ExternalDescription { get; set; }
        public int Length { get; set; }
        public bool IsRequired { get; set; }
        public string Separator { get; set; }
    }
}