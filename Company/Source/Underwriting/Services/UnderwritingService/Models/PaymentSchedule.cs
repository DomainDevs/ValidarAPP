using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    public class CompanyPaymentSchedule : BasePaymentSchedule
    {
        /// <summary>
        /// <summary>
        /// Distribución
        /// </summary>
        [DataMember]
        public virtual PaymentDistribution Distribution { get; set; }
    }
}
