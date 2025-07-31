using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Dirección de Notificación
    /// </summary>
    [DataContract]
    public class ProviderPaymentConcept : BaseProviderPaymentConcept
    {

        /// <summary>
        /// Identificador Conceptos de pago
        /// </summary>
        [DataMember]
        public PaymentConcept PaymentConcept { get; set; }
    }
}