using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyPaymentPlan: BasePaymentPlan
    {
        /// <summary>
        /// Cuotas
        /// </summary>
        [DataMember]
        public virtual List<Quota> Quotas { get; set; }

        /// <summary>
        /// Prima de financionación - Pagare
        /// </summary>
        [DataMember]
        public CompanyPremiumFinance PremiumFinance { get; set; }

        
    }
}
