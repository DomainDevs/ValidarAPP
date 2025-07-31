using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class PaymentRequestReportDTO
    {
        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string ReportDate { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string ContractCity { get; set; }

        [DataMember]
        public string PolicyAgent { get; set; }

        [DataMember]
        public string Prefix { get; set; }

        [DataMember]
        public string PolicyNumber { get; set; }

        [DataMember]
        public string ClaimNumber { get; set; }

        [DataMember]
        public string ClaimRegistrationDate { get; set; }

        [DataMember]
        public string PolicyHolder { get; set; }

        [DataMember]
        public string PolicyInsured { get; set; }

        [DataMember]
        public string PaymentBeneficiaryName { get; set; }

        [DataMember]
        public string PaymentBeneficiaryPersonType { get; set; }

        [DataMember]
        public string PaymentBeneficiaryDocumentNumber { get; set; }

        [DataMember]
        public string PaymentTechnicalTransaction { get; set; }

        [DataMember]
        public string VoucherType { get; set; }

        [DataMember]
        public string PaymentMethod { get; set; }

        [DataMember]
        public string PaymentCurrency { get; set; }

        [DataMember]
        public string VoucherCurrency { get; set; }

        [DataMember]
        public string CostCenter { get; set; }

        [DataMember]
        public string PaymentTotalAmount { get; set; }

        [DataMember]
        public string TRM { get; set; }

        [DataMember]
        public string TotalAmountConcepts { get; set; }

        [DataMember]
        public string PaymentDescription { get; set; }

        [DataMember]
        public List<ClaimReportDTO> Claims { get; set; }

        [DataMember]
        public List<TaxReportDTO> Taxes { get; set; }

        [DataMember]
        public List<AccountingReportDTO> Accountings { get; set; }

        [DataMember]
        public List<CoInsuranceReportDTO> Coinsurances { get; set; }
    }

    [DataContract]
    public class ClaimReportDTO
    {
        [DataMember]
        public string ClaimNumber { get; set; }

        [DataMember]
        public string SubClaim { get; set; }

        [DataMember]
        public string BusinessTurn { get; set; }

        [DataMember]
        public string Coverage { get; set; }

        [DataMember]
        public string Deducible { get; set; }

        [DataMember]
        public string Compensation { get; set; }

        [DataMember]
        public string Expenses { get; set; }

        [DataMember]
        public string Reinsurance { get; set; }
    }

    [DataContract]
    public class TaxReportDTO
    {
        [DataMember]
        public string TaxCode { get; set; }

        [DataMember]
        public string TaxCategory { get; set; }

        [DataMember]
        public string TaxDescription { get; set; }

        [DataMember]
        public string TaxBaseAmount { get; set; }

        [DataMember]
        public string TaxValue { get; set; }
    }

    [DataContract]
    public class AccountingReportDTO
    {
        [DataMember]
        public string Account { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string DebitAmount { get; set; }

        [DataMember]
        public string CreditAmount { get; set; }
    }

    [DataContract]
    public class CoInsuranceReportDTO
    {
        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string Participation { get; set; }

        [DataMember]
        public string Amount { get; set; }
    }
}
