
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class CheckBookControlModel
    {
        public int Id { get; set; }
        public int CheckFrom { get; set; }
        public int CheckTo { get; set; }
        public int LastCheck { get; set; }
        public int StatusId { get; set; }
        public string DateLow { get; set; }
        public int BankId { get; set; }
        public int BranchId { get; set; }
        public int AccountBankId { get; set; }
        public int IsAutomatic { get; set; }
        public int IsAutomaticId { get; set; }
        public int State { get; set; }
        public string DisabledDate { get; set; }
        public string Description { get; set; }
        public string SmallDescriptionBranch { get; set; }
        public string Number { get; set; }
        public string DescriptionIsAutomatic { get; set; }
        public string DescriptionState { get; set; }
        public string User { get; set; }
    }
}