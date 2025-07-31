using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class PaymentRequestDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TemporalId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int AccountBankId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public DateTime EstimatedDate { get; set; }
        
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }

        [DataMember]
        public int? BeneficiaryDocumentType { get; set; }

        [DataMember]
        public string BeneficiaryDocumentTypeDescription { get; set; }

        [DataMember]
        public string BeneficiaryFullName { get; set; }

        [DataMember]
        public bool IsPrinted { get; set; }

        [DataMember]
        public int MovementTypeId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public DateTime? PaymentDate { get; set; }

        [DataMember]
        public int PersonTypeId { get; set; }

        [DataMember]
        public int LineBusinessId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public decimal TotalTax { get; set; }

        [DataMember]
        public int PaymentRequestTypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int PaymentSourceId { get; set; }

        [DataMember]
        public int PaymentMethodId { get; set; }

        [DataMember]
        public string PaymentMethodDescription { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }

        /* Claims */

        [DataMember]
        public List<SubClaimDTO> Claims { get; set; }

        [DataMember]
        public int ClaimNumber { get; set; }

        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        [DataMember]
        public int PolicyProductId { get; set; }

        [DataMember]
        public int PolicySalePointId { get; set; }

        [DataMember]
        public string CoverageDescription { get; set; }

        [DataMember]
        public DateTime? EstimationDate { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public string EstimationTypeDescription { get; set; }

        [DataMember]
        public int EstimationTypeStatusId { get; set; }

        [DataMember]
        public int EstimationTypeStatusReasonId { get; set; }

        [DataMember]
        public int EstimationCurrencyId { get; set; }
        
        [DataMember]
        public string EstimationCurrencyDescription { get; set; }

        [DataMember]
        public decimal? EstimationAmount { get; set; }

        [DataMember]
        public string InsuredThird { get; set; }

        [DataMember]
        public int? RiskId { get; set; }

        [DataMember]
        public int? SalvageNumber { get; set; }

        [DataMember]
        public int? RecoveryNumber { get; set; }

        [DataMember]
        public int? ClaimBranchId { get; set; }

        [DataMember]
        public string ClaimBranchDescription { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public int AccountingTransaction { get; set; }

        [DataMember]
        public bool IsEnabledGeneralLedger { get; set; }

        [DataMember]
        public string SaveDailyEntryMessage { get; set; }
                
        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public bool IsTotal { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int BusinessTypeId { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public List<CoInsuranceDTO> CoInsurance { get; set; }
    }
}
