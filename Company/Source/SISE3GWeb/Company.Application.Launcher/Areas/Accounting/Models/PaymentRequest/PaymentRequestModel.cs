using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.PaymentRequest
{
    [KnownType("PaymentRequestModel")]
    public class PaymentRequestModel
    {
        public int PaymentRequestId { get; set; }
        public int PaymentSourceId { get; set; }
        public int BranchId { get; set; }
        public DateTime PaymentEstimateDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PersonTypeId { get; set; }
        public int IndividualId { get; set; }
        public int PaymentMethodId { get; set; }
        public int CurrencyId { get; set; }
        public int UserId { get; set; }
        public bool IsPrinted { get; set; }
        public double TotalAmount { get; set; }
        public string Description { get; set; }
        public int PaymentMovementTypeId { get; set; }
        public int PrefixId { get; set; }
        public int PersonBankAccountId { get; set; }
        public int PaymentRequestTypeId { get; set; }
        public int CompanyId { get; set; }
        public int SalePointId { get; set; }
        public List<VoucherModel> Vouchers { get; set; }

    }
    
    
    [KnownType("VoucherModel")]
    public class VoucherModel
    {
        public int VoucherId { get; set; }
        public int VoucherType { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public decimal VoucherExchangeRate { get; set; }
        public int VoucherCurrencyId { get; set; }
        public List<VoucherConceptModel> VoucherConcepts { get; set; }
    }

    [KnownType("VoucherConceptModel")]
    public class VoucherConceptModel
    {
        public int VoucherConceptId { get; set; }
        public int VoucherConceptPaymentConcept { get; set; }
        public decimal VoucherConceptValue { get; set; }
        public decimal VoucherConceptTaxValue { get; set; }
        public decimal VoucherConceptRetentionValue { get; set; }
        public int VoucherConceptCostCenterId { get; set; }
        public List<VoucherConceptTaxModel> VoucherConceptTaxes { get; set; }
    }

    [KnownType("VoucherConceptTaxModel")]
    public class VoucherConceptTaxModel
    {
        public int TaxId { get; set; }
        public int TaxCategoryId { get; set; }
        public int TaxConditionId { get; set; }
        public string TaxCategoryDescription { get; set; }
        public decimal TaxBase { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxValue { get; set; }
    }
}