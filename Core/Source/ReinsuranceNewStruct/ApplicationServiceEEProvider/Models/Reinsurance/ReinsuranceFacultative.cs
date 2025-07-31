using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsuranceFacultative
    {
        [DataMember]
        public int TempFacultativeCompanyId { get; set; }
        [DataMember]
        public int TempFacultativeId { get; set; }
        [DataMember]
        public int BrokerReinsuranceId { get; set; }
        [DataMember]
        public string BrokerDescription { get; set; }
        [DataMember]
        public int ReinsuranceCompanyId { get; set; }
        [DataMember]
        public string DescriptionCompany { get; set; }
        [DataMember]
        public string ParticipationPercentage { get; set; }
        [DataMember]
        public string PremiumPercentage { get; set; }
        [DataMember]
        public string CommissionPercentage { get; set; }
        [DataMember]
        public string SumValueParticipation { get; set; }
        [DataMember]
        public string SumValuePremium { get; set; }

        /// <summary>
        /// DepositPercentage
        /// </summary>
        [DataMember]
        public decimal DepositPercentage { get; set; }

        /// <summary>
        /// InterestOnReserve
        /// </summary>
        [DataMember]
        public decimal InterestOnReserve { get; set; }

        /// <summary>
        /// DepositReleaseDate
        /// </summary>
        [DataMember]
        public DateTime DepositReleaseDate { get; set; }
    }
}
