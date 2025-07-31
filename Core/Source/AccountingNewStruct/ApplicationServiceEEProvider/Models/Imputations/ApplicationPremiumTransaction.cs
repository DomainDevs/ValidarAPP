using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationPremiumTransaction : TransactionType
    {
        /// <summary>
        /// Lista de polizas y comisiones de la imputación
        /// </summary>        
        public List<ApplicationPremiumTransactionItem> PremiumReceivableItems { get; set; }
    }
}
