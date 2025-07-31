
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class DesignFormatModels
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int BankNetworkId { get; set; }
        public int FormatTypeId { get; set; }
        public int FileUsingId { get; set; }
        public string Separator { get; set; }
        public string DateFrom { get; set; }
        public int FormatId { get; set; }
    }
}