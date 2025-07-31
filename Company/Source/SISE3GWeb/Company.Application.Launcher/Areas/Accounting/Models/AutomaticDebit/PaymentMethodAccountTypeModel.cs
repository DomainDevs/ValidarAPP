namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit
{
    public class PaymentMethodAccountTypeModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeDescription { get; set; }
        public string DebitCode { get; set; }
    }
}