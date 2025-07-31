using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    /// <summary>
    /// Pago en tarjeta de credito
    /// </summary>
    [DataContract]
    public class CreditCard : Payment
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
        public CreditCardType Type { get; set; }

        /// <summary>
        /// Banco Emisor
        /// </summary>
        [DataMember]
        public Bank IssuingBank { get; set; }

        /// <summary>
        /// Validez de la tarjeta
        /// </summary>
        [DataMember]
        public CreditCardValidThru ValidThru { get; set; }
    }
}
