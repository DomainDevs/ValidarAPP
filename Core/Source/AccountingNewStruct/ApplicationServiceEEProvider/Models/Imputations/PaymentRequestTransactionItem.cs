using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// PaymentRequestTransactionItem:  Solicitud de Pago 
    /// </summary>
    [DataContract]
    public class PaymentRequestTransactionItem
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
        public PaymentRequest PaymentRequest { get; set; }
               
              
    }
}
