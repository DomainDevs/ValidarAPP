namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class FieldFormatModels
    {
        public int Id { get; set; }
        public int FormatId { get; set; }
        public int ColumnNumber { get; set; }
        public string Description { get; set; }
        public string ExternalDescription { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string Value { get; set; }
        public string Mask { get; set; }
        public string Align { get; set; }
        public string Filled { get; set; }
    }
}