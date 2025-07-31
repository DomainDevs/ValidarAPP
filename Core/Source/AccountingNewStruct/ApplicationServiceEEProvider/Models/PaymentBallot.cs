using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;

//Sistran
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    /// <summary>p
    /// PaymentBallot: boleta de pagos
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class PaymentBallot
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// BallotNumber: Numero de Boleta 
        /// </summary>        
        [DataMember]
        public string BallotNumber { get; set; }

        /// <summary>
        /// Bank: Banco
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
        /// BankDate: Fecha de Boleta de Banco
        /// </summary>        
        [DataMember]
        public DateTime BankDate { get; set; }

        /// <summary>
        /// Amount: Total de todos los pagos del ticket
        /// </summary>        
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// BankAmount: Total de todos los pagos del ticket del banco
        /// </summary>        
        [DataMember]
        public Amount BankAmount { get; set; }

        /// <summary>
        /// State: Estado 1 activo , 0 inactivo 
        /// </summary>        
        [DataMember]
        public int Status { get; set; }


        /// <summary>
        /// Payments: Pagos efectuados al Ticket
        /// </summary>        
        [DataMember]
        public List<PaymentTicket> PaymentTickets { get; set; }

        /// <summary>
        /// Transaction: Transaccion
        /// </summary>        
        [DataMember]
        public Transaction Transaction { get; set; }
    }
}
