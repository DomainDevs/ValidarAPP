using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// ClaimsPaymentRequestTransactionItem:  Solicitud de Pago de Claims, listado
    /// </summary>
    [DataContract]
    public class ClaimsPaymentRequestTransactionItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PaymentRequest: Solicitud de Pago 
        /// </summary>        
        [DataMember]
        public PaymentRequestDTO PaymentRequest { get; set; }

        /// <summary>
        /// BussinessType : Tipo de Negocio 
        /// </summary>        
        [DataMember]
        public int BussinessType { get; set; }
    }
}
