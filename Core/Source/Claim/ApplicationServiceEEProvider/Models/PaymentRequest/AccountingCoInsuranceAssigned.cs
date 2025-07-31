using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class AccountingCoInsuranceAssigned
    {
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int InsuranceCompanyId { get; set; }

        [DataMember]
        public decimal PartCiaPercentage { get; set; }

        [DataMember]
        public decimal ExpensesPercentage { get; set; }

        [DataMember]
        public int CompanyNum { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public string AccountingAccountNumber { get; set; }

        [DataMember]
        public int AccountingNatureId { get; set; }
    }
}
