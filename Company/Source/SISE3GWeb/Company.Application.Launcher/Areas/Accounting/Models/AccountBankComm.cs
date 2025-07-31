
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class AccountBankComm
    {
        public int Id { get; set; }
        public int AccountTypeId { get; set; }
        public string Number { get; set; }
        public int BankId { get; set; }
        public int Enabled { get; set; }
        public int Default { get; set; }
        public int CurrencyId { get; set; }
        public int GeneralLedgerId { get; set; }
        public string DisabledDate { get; set; }
        public int BranchId { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Campo agregado para que soporte el nro de cuenta en el BE
        /// Autor: Saidel Concepcion
        /// </summary>
        public string GeneralLedgerNumber { get; set; }
    }
}
