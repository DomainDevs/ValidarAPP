using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    /// <summary>
    /// alejo
    /// TransferPaymentOrder
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class TransferPaymentOrderDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// BankAccountCompany: Cuenta del Banco de la Compañia
        /// </summary>        
        [DataMember]
        public BankAccountCompanyDTO BankAccountCompany { get; set; }

        /// <summary>
        /// DeliveryDate: fecha de envío
        /// </summary>        
        [DataMember]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// CancellationDate: fecha de anulación de la transferencia
        /// </summary>        
        [DataMember]
        public DateTime? CancellationDate { get; set; }

        /// <summary>
        /// Status: Estado
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// User: Usuario que modifica la transferencia
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }
        
        /// <summary>
        /// PaymentOrders: Ordenes de Pago asociadas
        /// </summary>        
        [DataMember]
        public List<PaymentOrderDTO> PaymentOrders { get; set; }
    }
}
