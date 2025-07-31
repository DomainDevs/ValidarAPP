using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Metodos de pago
    /// </summary>
    [DataContract]
    public class PaymentMethodAccount : BasePaymentMethodAccount
    {
        /// <summary>
        /// Forma de pago
        /// </summary>
        [DataMember]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Banco
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }
        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        [DataMember]
        public PaymentAccountType AccountType { get; set; }
    }
}
