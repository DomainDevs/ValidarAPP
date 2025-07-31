using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class FinancialPlan : BaseFinancialPlan
    {

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Gets or sets the payment schedule.
        /// </summary>
        /// <value>
        /// The payment schedule.
        /// </value>
        [DataMember]
        public PaymentSchedule PaymentSchedule { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        [DataMember]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
