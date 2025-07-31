using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// BrokersCheckingAccountTransaction:  Transaccion de Cuenta Corriente Agentes
    /// </summary>
    [DataContract]
    public class BrokersCheckingAccountTransactionDTO: TransactionTypeDTO
    {
        /// <summary>
        /// BrokersCheckingAccountTransactionItems:  Items Cuenta Corriente
        /// </summary>        
        [DataMember]
        public List<BrokersCheckingAccountTransactionItemDTO> BrokersCheckingAccountTransactionItems  { get; set; }
    }
}
