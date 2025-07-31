using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// ClaimsPaymentRequestTransaction:  Solicitud de Pago de Siniestros
    /// </summary>
    [DataContract]
    public class ClaimsPaymentRequestTransaction : TransactionType
    {
        /// <summary>
        /// ClaimsPaymentRequestItems: Lista de solicitud de Pagos
        /// </summary>        
        [DataMember]
        public List<ClaimsPaymentRequestTransactionItem> ClaimsPaymentRequestItems { get; set; }
    }
}
