using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Planes de Pago
    /// </summary>
    [DataContract]
    public class PaymentPlan : BasePaymentPlan
    {
        /// <summary>
        /// Cuotas
        /// </summary>
        [DataMember]
        public virtual List<Quota> Quotas { get; set; }
    }
}