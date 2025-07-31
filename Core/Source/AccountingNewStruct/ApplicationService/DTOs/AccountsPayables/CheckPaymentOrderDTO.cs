using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    /// <summary>
    /// CheckPaymentOrder
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CheckPaymentOrderDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// BankAccountCompany: Cuenta del Banco la Compañia
        /// </summary>        
        [DataMember]
        public BankAccountCompanyDTO BankAccountCompany { get; set; }

        /// <summary>
        /// CheckNumber: Numero de Cheque
        /// </summary>        
        [DataMember]
        public int CheckNumber { get; set; }

        /// <summary>
        /// IsCheckPrinted: Cheque Impreso
        /// </summary>        
        [DataMember]
        public bool IsCheckPrinted { get; set; }

        /// <summary>
        /// User: Usuario que imprimio el cheque
        /// </summary>        
        [DataMember]
        public int PrintedUser { get; set; }

        /// <summary>
        /// DatePrinted: fecha de impresion del cheque
        /// </summary>        
        [DataMember]
        public DateTime? PrintedDate { get; set; }
        
        /// <summary>
        /// DeliveryDate: fecha de entrega del cheque
        /// </summary>        
        [DataMember]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// PersonType: Tipo de Persona
        /// </summary>        
        [DataMember]
        public PersonTypeDTO PersonType { get; set; }

        /// <summary>
        /// Delivery: Individio que recibe el cheque
        /// </summary>        
        [DataMember]
        public IndividualDTO Delivery { get; set; }

        /// <summary>
        /// Status: Estado
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// RefoundDate: Fecha de devolución
        /// </summary>        
        [DataMember]
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// CancellationDate: Fecha de Cancelacion
        /// </summary>        
        [DataMember]
        public DateTime? CancellationDate { get; set; }

        /// <summary>
        /// CancellationUser: Usuario de Cancelacion 
        /// </summary>        
        [DataMember]
        public int CancellationUser { get; set; }

        /// <summary>
        /// CourierName: Persona que retira el cheque
        /// </summary>        
        [DataMember]
        public string CourierName { get; set; }

        /// <summary>
        /// PaymentOrders: Ordenes de Pago asociadas
        /// </summary>        
        [DataMember]
        public List<PaymentOrderDTO> PaymentOrders { get; set; }

    }
}
