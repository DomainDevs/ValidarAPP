using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// PaymentRequestTransaction:  Solicitud de Pago Varios
    /// </summary>
    [DataContract]
    public class PaymentRequestTransactionDTO : TransactionTypeDTO
    {
        /// <summary>
        /// PaymentRequestItems: Lista de solicitud de Pagos Varios
        /// </summary>        
        [DataMember]
        public List<PaymentRequestTransactionItemDTO> PaymentRequestItems { get; set; }
    }
}
