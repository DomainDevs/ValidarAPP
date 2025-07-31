using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// ClaimsPaymentRequestTransaction:  Solicitud de Pago de Siniestros
    /// </summary>
    [DataContract]
    public class ClaimsPaymentRequestTransactionDTO : TransactionTypeDTO
    {
        /// <summary>
        /// ClaimsPaymentRequestItems: Lista de solicitud de Pagos
        /// </summary>        
        [DataMember]
        public List<ClaimsPaymentRequestTransactionItemDTO> ClaimsPaymentRequestItems { get; set; }
    }
}
