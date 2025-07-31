using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class PolicyDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public decimal DocumentNumber { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int EndorsementTypeId { get; set; }

        [DataMember]
        public int HolderId { get; set; }

        [DataMember]
        public string HolderDocumentNumber { get; set; }

        [DataMember]
        public string HolderName { get; set; }

        [DataMember]
        public string Intermediary { get; set; }

        [DataMember]
        public string EndorsementType { get; set; }

        [DataMember]
        public string BusinessType { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public string PendingDebt { get; set; }

        [DataMember]
        public string ClaimsQuantity { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int BusinessTypeId { get; set; }

        [DataMember]
        public int Inforce { get; set; }

        [DataMember]
        public string IssueDate { get; set; }

        [DataMember]
        public decimal Premium { get; set; }

        [DataMember]
        public int PolicyTypeId { get; set; }

        [DataMember]
        public string PolicyType { get; set; }

        [DataMember]
        public decimal TaxExpenses { get; set; }

        [DataMember]
        public decimal FullPremium { get; set; }

        [DataMember]
        public DateTime? CurrentFrom { get; set; }

        [DataMember]
        public DateTime? CurrentTo { get; set; }

        [DataMember]
        public string Agent { get; set; }

        [DataMember]
        public string AgentTypeDescription { get; set; }

        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public string BilledPeriodFrom { get; set; }

        [DataMember]
        public string BilledPeriodTo { get; set; }

        [DataMember]
        public List<CoInsuranceDTO> CoInsurance { get; set; }

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public string RiskDescription { get; set; }

        [DataMember]
        public int EndorsementDocumentNum { get; set; }

        [DataMember]
        public int PersonTypeId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProducDescription { get; set; }

        [DataMember]
        public int SalePointId { get; set; }

        [DataMember]
        public bool IsReinsurance { get; set; }
    }
}
