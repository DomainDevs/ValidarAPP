using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class ChargeRequestDTO
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
        public DateTime EstimatedDate { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public bool IsPrinted { get; set; }

        [DataMember]
        public int MovementTypeId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int PersonTypeId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public int PaymentRequestTypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int PaymentSourceId { get; set; }

        [DataMember]
        public int PaymentMethodId { get; set; }

        [DataMember]
        public VoucherDTO Voucher { get; set; }

        [DataMember]
        public int? ClaimId { get; set; }

        [DataMember]
        public int? SubClaim { get; set; }

        [DataMember]
        public int? ClaimNumber { get; set; }

        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        [DataMember]
        public int? RiskId { get; set; }

        [DataMember]
        public int? SalvageId { get; set; }

        [DataMember]
        public int? RecoveryId { get; set; }

        [DataMember]
        public int? ClaimBranchId { get; set; }

        [DataMember]
        public string ClaimBranchDescription { get; set; }

        [DataMember]
        public int BeneficiaryDocumentType { get; set; }

        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }

        [DataMember]
        public string BeneficiaryFullName { get; set; }

        [DataMember]
        public decimal RecoveryOrSalvageAmount { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }

    }
}
