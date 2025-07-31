using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    /// Pago en cheque
    /// </summary>   
    [DataContract]
    public class CheckDTO : PaymentDTO
    {
        /// <summary>
        /// Número de documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Banco Emisor
        /// </summary>
        [DataMember]
        public BankDTO IssuingBank { get; set; }

        /// <summary>
        /// Número de cuenta Emisor
        /// </summary>
        [DataMember]
        public string IssuingAccountNumber { get; set; }

        /// <summary>
        /// Nombre Emisor del cheque
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }
    }
}
