using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyClaimsBills
    {
        [DataMember]
        public bool HasTotalLoss { get; set; }

        [DataMember]
        public int SinisterQuantity { get; set; }

        [DataMember]
        public decimal PortfolioBalance { get; set; }

        [DataMember]
        public int RenewalsQuantity { get; set; }

        [DataMember]
        public int ClaimsQuantityLastYears { get; set; }

    }
}
