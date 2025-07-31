using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// PaymentRequestTransactionItem:  Solicitud de Pago 
    /// </summary>
    [DataContract]
    public class PaymentRequestTransactionItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PaymentRequest 
        /// </summary>        
        [DataMember]
        public PaymentRequestDTO PaymentRequest { get; set; }
               
              
    }
}
