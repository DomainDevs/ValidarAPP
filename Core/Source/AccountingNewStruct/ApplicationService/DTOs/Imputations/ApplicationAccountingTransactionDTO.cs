using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class ApplicationAccountingTransactionDTO : TransactionTypeDTO
    {
        /// <summary>
        /// Lista de movimientos contables
        /// </summary>        
        [DataMember]
        public List<ApplicationAccountingDTO> ApplicationAccountingItems { get; set; }
    }
}
