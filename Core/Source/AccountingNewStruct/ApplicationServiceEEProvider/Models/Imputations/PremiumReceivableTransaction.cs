using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// PremiumsReceivable:  Primas por cobrar
    /// </summary>
    [DataContract]
    public class PremiumReceivableTransaction: TransactionType
    {
        /// <summary>
        /// PremiumReceivableItems:Lista de polizas y comisiones de la imputación
        /// </summary>        
        [DataMember]
        public List<PremiumReceivableTransactionItem> PremiumReceivableItems { get; set; }
    }
}
