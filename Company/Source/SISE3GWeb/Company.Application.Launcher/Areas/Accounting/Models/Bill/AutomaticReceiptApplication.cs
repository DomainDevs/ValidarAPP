using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("AutomaticReceiptApplication")]
    public class AutomaticReceiptApplication
    {
        public PremiumReceivableModel premiumReceivable { get; set; }
        public UsedDepositPremiumModel usedDepositPremiumModel { get; set; }
        public List<AccountingTransactionModel> accountingTransactionModel { get; set; }
    }

}