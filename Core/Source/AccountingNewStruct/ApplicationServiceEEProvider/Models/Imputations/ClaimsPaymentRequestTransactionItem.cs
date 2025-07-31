using Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// ClaimsPaymentRequestTransactionItem:  Solicitud de Pago de Claims, listado
    /// </summary>
    [DataContract]
    public class ClaimsPaymentRequestTransactionItem
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
        public PaymentRequest PaymentRequest { get; set; }

        /// <summary>
        /// BussinessType : Tipo de Negocio 
        /// </summary>        
        [DataMember]
        public int BussinessType { get; set; }
    }
}
