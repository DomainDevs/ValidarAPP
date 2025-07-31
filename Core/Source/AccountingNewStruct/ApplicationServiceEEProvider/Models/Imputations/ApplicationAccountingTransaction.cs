using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationAccountingTransaction : TransactionType
    {
        /// <summary>
        /// Movimientos contables
        /// </summary>
        public List<ApplicationAccounting> ApplicationAccountingItems { get; set; }
    }
}
