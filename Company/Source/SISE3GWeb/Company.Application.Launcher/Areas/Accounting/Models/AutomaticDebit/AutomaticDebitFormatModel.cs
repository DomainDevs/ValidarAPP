
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class AutomaticDebitFormatModel
    {
        public int Id { get; set; }

        public int BankNetworkId { get; set; }

        public int FormatId { get; set; }

        public string FileUsing { get; set; }

        public string OperationType { get; set; }
    }
}