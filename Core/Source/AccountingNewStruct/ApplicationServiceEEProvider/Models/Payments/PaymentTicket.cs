using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    /// <summary>
    /// PaymentTicket: ticket interno de pagos
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class PaymentTicket
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
        public Amount Amount { get; set; }

        /// <summary>
        /// Commission: Total de las comisiones
        /// </summary>        
        [DataMember]
        public Amount Commission { get; set; }

        /// <summary>
        /// Branch: Sucursal de Boleta
        /// </summary>        
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// ItemsTotal: Total de todos los items
        /// </summary>        
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// AccountNumber: Numero de Cuenta
        /// </summary>        
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Currency: Moneda
        /// </summary>        
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// CashAmount: Total Efectivo
        /// </summary>        
        [DataMember]
        public Amount CashAmount { get; set; }

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
        public List<Payment> Payments { get; set; }

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
