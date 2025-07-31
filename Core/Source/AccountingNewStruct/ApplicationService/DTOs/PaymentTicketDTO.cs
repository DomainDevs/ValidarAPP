using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>dd
    /// PaymentTicket: ticket interno de pagos
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class PaymentTicketDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PaymentsTotal: Total de todos los pagos del ticket
        /// </summary>        
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// Commission: Total de las comisiones
        /// </summary>        
        [DataMember]
        public AmountDTO Commission { get; set; }

        /// <summary>
        /// Branch: Sucursal de Boleta
        /// </summary>        
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// ItemsTotal: Total de todos los items
        /// </summary>        
        [DataMember]
        public BankDTO Bank { get; set; }

        /// <summary>
        /// AccountNumber: Numero de Cuenta
        /// </summary>        
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Currency: Moneda
        /// </summary>        
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        /// CashAmount: Total Efectivo
        /// </summary>        
        [DataMember]
        public AmountDTO CashAmount { get; set; }

        /// <summary>
        /// PaymentMethod: Metodo de Pago
        /// </summary>        
        [DataMember]
        public int PaymentMethod { get; set; }

        /// <summary>
        /// State: Estado 1 activo , 0 inactivo 
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// Payments: Pagos efectuados al Ticket
        /// </summary>        
        [DataMember]
        public List<PaymentDTO> Payments { get; set; }

        /// <summary>
        /// DiabledUser: Codigo Usuario  que desabilita 
        /// </summary>        
        [DataMember]
        public int? DisabledUser { get; set; }

        /// <summary>
        /// DiabledDate: Fecha de baja de recibo
        /// </summary>        
        [DataMember]
        public DateTime? DisabledDate { get; set; }
    }
}
