using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// BrokersCheckingAccountTransaction:  Transaccion de Cuenta Corriente Agentes
    /// </summary>
    [DataContract]
    public class BrokersCheckingAccountTransaction : TransactionType
    {
        /// <summary>
        /// BrokersCheckingAccountTransactionItems:  Items Cuenta Corriente
        /// </summary>        
        [DataMember]
        public List<BrokersCheckingAccountTransactionItem> BrokersCheckingAccountTransactionItems  { get; set; }
    }
}
