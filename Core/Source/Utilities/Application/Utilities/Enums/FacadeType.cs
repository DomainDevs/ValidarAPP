using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Utilities.Enums
{
    /// <summary>
    /// Este ENUM se llena de manera dinamica desde la tabla PARAM.ENUM_PARAMETER
    /// </summary>
    [DataContract]
    [Flags]
    public enum FacadeType
    {
        [Description("FacadeLog")]
        RULE_FACADE_LOG,

        [Description("FacadeGeneral")]
        RULE_FACADE_GENERAL,

        [Description("FacadeRisk")]
        RULE_FACADE_RISK,

        [Description("FacadeCoverage")]
        RULE_FACADE_COVERAGE,

        [Description("FacadeComponent")]
        RULE_FACADE_COMPONENT,

        [Description("FacadeCommission")]
        RULE_FACADE_COMMISSION,

        [Description("FacadeEvent")]
        RULE_FACADE_EVENT,

        [Description("FacadeClaimNotice")]
        RULE_FACADE_CLAIM_NOTICE,

        [Description("FacadeClaim")]
        RULE_FACADE_CLAIM,

        [Description("FacadeEstimation")]
        RULE_FACADE_ESTIMATION,

        [Description("FacadePaymentRequest")]
        RULE_FACADE_PAYMENT_REQUEST,

        [Description("FacadeChargeRequest")]
        RULE_FACADE_CHARGE_REQUEST,

        [Description("FacadeVoucher")]
        RULE_FACADE_VOUCHER,

        [Description("FacadeVoucherConcept")]
        RULE_FACADE_VOUCHER_CONCEPT,

        [Description("FacadeGeneralPerson")]
        RULE_FACADE_GENERAL_PERSON,

        [Description("FacadeInsured")]
        RULE_FACADE_INSURED,

        [Description("FacadeProvider")]
        RULE_FACADE_PROVIDER,

        [Description("FacadeThird")]
        RULE_FACADE_THIRD,

        [Description("FacadeIntermediary")]
        RULE_FACADE_INTERMEDIARY,

        [Description("FacadeEmployed")]
        RULE_FACADE_EMPLOYED,

        [Description("FacadePersonalInf")]
        RULE_FACADE_PERSONAL_INF,

        [Description("FacadePaymentMethods")]
        RULE_FACADE_PAYMENT_METHODS,

        [Description("FacadeGuarantees")]
        RULE_FACADE_GUARANTEES,

        [Description("FacadeOperatingQuota")]
        RULE_FACADE_OPERATING_QUOTA,

        [Description("FacadeTaxes")]
        RULE_FACADE_TAXES,

        [Description("FacadeBankTransfers")]
        RULE_FACADE_BANK_TRANSFERS,

        [Description("FacadeReinsurer")]
        RULE_FACADE_REINSURER,

        [Description("FacadeCoinsurer")]
        RULE_FACADE_COINSURER,

        [Description("FacadeConsortiates")]
        RULE_FACADE_CONSORTIATES,

        [Description("FacadeBusinessName")]
        RULE_FACADE_BUSINESS_NAME,

        [Description("FacadeGeneralSarlaft")]
        RULE_FACADE_GENERAL_SARLAFT,

        [Description("FacadeLinks")]
        RULE_FACADE_LINKS,

        [Description("FacadeInternationalOperations")]
        RULE_FACADE_INTERNATIONAL_OPERATIONS,

        [Description("FacadeLegalRepresentative")]
        RULE_FACADE_LEGAL_REPRESENTATIVE,

        [Description("FacadePartners")]
        RULE_FACADE_PARTNERS,

        [Description("FacadeBeneficiaries")]
        RULE_FACADE_BENEFICIARIES,

        [Description("FacadeGeneralBasicInfo")]
        RULE_FACADE_GENERAL_BASIC_INFO,
        
        [Description("FacadeGeneralAutomaticQuota")]
        RULE_FACADE_GENERAL_AUTOMATIC_QUOTA,
        
        [Description("FacadeThirdAutomaticQuota")]
        RULE_FACADE_THIRD_AUTOMATIC_QUOTA,

        [Description("FacadeBusinessAutomaticQuota")]
        RULE_FACADE_BUSINESS_AUTOMATIC_QUOTA
    }
}
