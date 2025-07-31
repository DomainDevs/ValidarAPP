using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class PaymentDistribution : BasePaymentDistribution
    {
        /// <summary>
        /// Tipo De Calculo De Pago
        /// </summary> 
        [DataMember]
        public virtual PaymentCalculationType? CalculationType { get; set; }
    }
}