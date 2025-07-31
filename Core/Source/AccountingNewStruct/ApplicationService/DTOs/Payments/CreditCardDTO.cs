using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
 
    /// <summary>
    /// Pago en tarjeta de credito
    /// </summary>
    [DataContract]
    public class CreditCardDTO : PaymentDTO
    {
        /// <summary>
        /// Número de Voucher
        /// </summary>
        [DataMember]
        public string Voucher { get; set; }

        /// <summary>
        /// Número de tarjeta de credito
        /// </summary>
        [DataMember]
        public string CardNumber { get; set; }

        /// <summary>
        /// Número de Autorización
        /// </summary>
        [DataMember]
        public string AuthorizationNumber { get; set; }

        /// <summary>
        /// Tipo de tarjeta de credito (Visa, MasterCard, etc)
        /// </summary>
        [DataMember]
        public CreditCardTypeDTO Type { get; set; }

        /// <summary>
        /// Banco Emisor
        /// </summary>
        [DataMember]
        public BankDTO IssuingBank { get; set; }
        

        /// <summary>
        /// Nombre tenedor tarjeta de credito
        /// </summary>
        [DataMember]
        public string Holder { get; set; }

        /// <summary>
        /// Validez de la tarjeta
        /// </summary>
        [DataMember]
        public CreditCardValidThruDTO ValidThru { get; set; }
    }
}
