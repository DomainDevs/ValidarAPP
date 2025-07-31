using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable
{
    [KnownType("PaymentRequestModel")]
    public class PaymentRequestModel
    {
        public int PaymentRequestInfoId { get; set; }
        public int PaymentRequestSource { get; set; }
        public int PaymentRequestBranch { get; set; }
        public DateTime PaymentRequestEstimatedDate { get; set; }
        public DateTime PaymentRequestRegistrationDate { get; set; }
        public DateTime PaymentRequestPaymentDate { get; set; }
        public int PaymentRequestPersonTypeId { get; set; }
        public int PaymentRequestIndividualId { get; set; }
        public int PaymentRequestPaymentMethodId { get; set; }
        public int PaymentRequestCurrencyId { get; set; }
        public int PaymentRequestUserId { get; set; }
        public bool PaymentRequestIsPrinted { get; set; }
        public double PaymentRequestTotalAmount { get; set; }
        public string PaymentRequestDescription { get; set; }
        public int PaymentRequestPaymentMovementTypeId { get; set; }
        public int PaymentRequestPrefixId { get; set; }
        public int PaymentRequestAccountBankId { get; set; }
        public int PaymentRequestType { get; set; }
        public List<PaymentTaxCategoryModel> PaymentTax { get; set; }
        public List<PaymentClaimModel> PaymentClaim { get; set; }
        public int PaymentRequestCompanyId { get; set; }
        public int PaymentRequestSalePointId { get; set; }

        #region PaymentRequestInfo

        [KnownType("PaymentTaxCategoryModel")]
        public class PaymentTaxCategoryModel
        {
            public int TaxId { get; set; }
            public int TaxCategoryId { get; set; }
            public int TaxCondition { get; set; }
            public string TaxCategoryDescript { get; set; }
        }

        [KnownType("PaymentClaimModel")]
        public class PaymentClaimModel
        {
            public int PaymentClaimId { get; set; }
            public List<PaymentClaimCoveragesModel> PaymentClaimCoverages { get; set; }
        }

        [KnownType("PaymentClaimCoveragesModel")]
        public class PaymentClaimCoveragesModel
        {
            public int PaymentClaimCoveragesId { get; set; }
            public int PaymentClaimCoveragesSubClaim { get; set; }
            public List<PaymentClaimCoveragesClaimAmountModel> PaymentClaimCoveragesClaimAmount { get; set; }
        }

        [KnownType("PaymentClaimCoveragesClaimAmountModel")]
        public class PaymentClaimCoveragesClaimAmountModel
        {
            public int PaymentClaimCoveragesClaimAmountId { get; set; }
            public int PaymentClaimCoveragesClaimAmountEstimationTypeId { get; set; }
            public int PaymentClaimCoveragesClaimAmountVersion { get; set; }
            public DateTime PaymentClaimCoveragesClaimAmountDate { get; set; }
            public decimal PaymentClaimCoveragesClaimAmountEstimatedAmount { get; set; }
            public decimal PaymentClaimCoveragesClaimAmountDeductibleAmount { get; set; }
            public int PaymentClaimCoveragesClaimAmountPaymentTypeId { get; set; }
            public List<VoucherModel> PaymentClaimCoveragesClaimAmountVoucher { get; set; }
        }


        [KnownType("VoucherConceptModel")]
        public class VoucherConceptModel
        {
            public int VoucherConceptId { get; set; }
            public int VoucherConceptPaymentConcept { get; set; }
            public double VoucherConceptValue { get; set; }
            public double VoucherConceptTaxValue { get; set; }
            public decimal VoucherConceptRetentionValue { get; set; }
            public int VoucherConceptCostCenterId { get; set; }
        }


        [KnownType("VoucherModel")]
        public class VoucherModel
        {
            public int VoucherId { get; set; }
            public int VoucherType { get; set; }
            public string VoucherNumber { get; set; }
            public DateTime VoucherDate { get; set; }
            public double VoucherExchangeRate { get; set; }
            public int VoucherCurrencyId { get; set; }
            public List<VoucherConceptModel> VoucherConcept { get; set; }
        }


        #endregion
    }
}