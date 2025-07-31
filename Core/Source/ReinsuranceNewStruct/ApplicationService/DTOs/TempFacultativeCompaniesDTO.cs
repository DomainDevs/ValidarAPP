using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class TempFacultativeCompaniesDTO
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

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public decimal FacultativePercentage { get; set; }

        [DataMember]
        public decimal FacultativePremiumPercentage { get; set; }
    }
}
