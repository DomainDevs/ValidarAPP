using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    /// <summary>
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
        public Decimal Amount { get; set; }

        /// <summary>
        /// Commission: Total de las comisiones
        /// </summary>        
        [DataMember]
        public Decimal Commission { get; set; }

        /// <summary>
        /// Branch: Sucursal de Boleta
        /// </summary>        
        [DataMember]
        public int BranchId { get; set; }


        /// <summary>
        /// Branch: Sucursal de Boleta
        /// </summary>        
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// BankId: Id del banco
        /// </summary>        
        [DataMember]
        public int BankId { get; set; }

        /// <summary>
        /// BankDescription: Descripcion del banco
        /// </summary>        
        [DataMember]
        public string BankDescription { get; set; }

        /// <summary>
        /// AccountNumber: Numero de Cuenta
        /// </summary>        
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Currency: Moneda
        /// </summary>        
        [DataMember]
        public int CurrencyId { get; set; }

        /// <summary>
        /// CashAmount: Total Efectivo
        /// </summary>        
        [DataMember]
        public decimal CashAmount { get; set; }

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
