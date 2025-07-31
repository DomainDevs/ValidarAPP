using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// PaymentRequestTransaction:  Solicitud de Pago Varios
    /// </summary>
    [DataContract]
    public class PaymentRequestTransaction : TransactionType
    {
        /// <summary>
        /// PaymentRequestItems: Lista de solicitud de Pagos Varios
        /// </summary>        
        [DataMember]
        public List<PaymentRequestTransactionItem> PaymentRequestItems { get; set; }
    }
}
