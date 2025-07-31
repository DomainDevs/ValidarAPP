using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Objeto del seguro
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UnderwritingServices.Models.InsuredObject" />
    [DataContract]
    public class CompanyInsuredObject : InsuredObject
    {
        [DataMember]
        public int BillingPeriodDepositPremium { get; set; }

        [DataMember]
        public int? DeclarationPeriod { get; set; }

        [DataMember]
        public decimal? LimitAmount { get; set; }

        [DataMember]
        public decimal? Rate { get; set; }

        [DataMember]
        public decimal? DepositPremiunPercent { get; set; }
    }
}