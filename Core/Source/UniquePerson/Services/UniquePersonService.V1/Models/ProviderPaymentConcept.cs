using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Dirección de Notificación
    /// </summary>
    [DataContract]
    public class SupplierPaymentConcept : Base.SupplierPaymentConcept
    {

        /// <summary>
        /// Identificador Conceptos de pago
        /// </summary>
        [DataMember]
        public PaymentConcept PaymentConcept { get; set; }
    }
}