
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class PaymentMethodBankNetworkModel
    {
        public int Id { get; set; }
        public int PaymentMethodId { get; set; }
        public int AccountBankId { get; set; }
        public bool ToGenerate { get; set; }
    }
}