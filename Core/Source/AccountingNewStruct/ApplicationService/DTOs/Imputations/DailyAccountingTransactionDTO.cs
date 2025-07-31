using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// DailyAccountingTransaction: Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingTransactionDTO  : TransactionTypeDTO
    {

        /// <summary>
        /// DailyAccountingTransactionItems: Lista de transacciones contables diarias
        /// </summary>        
        [DataMember]
        public List<DailyAccountingTransactionItemDTO> DailyAccountingTransactionItems { get; set; }
    }
}
