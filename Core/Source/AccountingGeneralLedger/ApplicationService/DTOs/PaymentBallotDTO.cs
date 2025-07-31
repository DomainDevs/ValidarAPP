using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class PaymentBallotDTO
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
        /// BankDate: Fecha de Boleta de Banco
        /// </summary>        
        [DataMember]
        public DateTime BankDate { get; set; }

        /// <summary>
        /// Amount: Total de todos los pagos del ticket
        /// </summary>        
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// BankAmount: Total de todos los pagos del ticket del banco
        /// </summary>        
        [DataMember]
        public AmountDTO BankAmount { get; set; }

        /// <summary>
        /// State: Estado 1 activo , 0 inactivo 
        /// </summary>        
        [DataMember]
        public int Status { get; set; }


        /// <summary>
        /// Payments: Pagos efectuados al Ticket
        /// </summary>        
        [DataMember]
        public List<PaymentTicketDTO> PaymentTickets { get; set; }

        /// <summary>
        /// Transaction: Transaccion
        /// </summary>        
        [DataMember]
        public TransactionDTO Transaction { get; set; }
    }
}
