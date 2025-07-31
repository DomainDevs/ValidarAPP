using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Planes de Pago
    /// </summary>
    [DataContract]
    public class PaymentSchedule : BasePaymentSchedule
    {
        /// <summary>
        /// Distribución
        /// </summary>
        [DataMember]
        public virtual PaymentDistribution Distribution { get; set; }
    }
}