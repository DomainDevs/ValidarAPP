using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Metodos de pago
    /// </summary>
    [DataContract]
    public class PaymentAccount : BasePaymentAccount
    {
        /// <summary>
        /// Banco sucursal 
        /// </summary>
        [DataMember]
        public BankBranch BankBranch { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        [DataMember]
        public PaymentAccountType Type { get; set; }

    }
}
