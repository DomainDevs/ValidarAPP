using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
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