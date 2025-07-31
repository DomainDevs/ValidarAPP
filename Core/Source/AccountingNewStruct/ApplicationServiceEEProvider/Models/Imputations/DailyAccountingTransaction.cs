using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// DailyAccountingTransaction: Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingTransaction : TransactionType
    {

        /// <summary>
        /// DailyAccountingTransactionItems: Lista de transacciones contables diarias
        /// </summary>        
        [DataMember]
        public List<DailyAccountingTransactionItem> DailyAccountingTransactionItems { get; set; }

    }
}
