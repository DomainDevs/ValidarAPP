using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Enums
{
    public enum AccountingKeys
    {
        [EnumMember]
        ACC_MODULE_ACCOUNTING,

        [EnumMember]
        ACC_MODULE_DATE_ACCOUNTING,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_CASH,

        [EnumMember]
        ACC_CHECKING_ACCOUNT_CONCEPTID,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_DATA_PHONE,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_DIRECT_CONECTION,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_TRANSFER,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_PAYMENT_AREA,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_PAYMENT_CARD,

        [EnumMember]
        ACC_CREDIT_AMORTIZATION,

        [EnumMember]
        ACC_DEBIT_AMORTIZATION,

        [EnumMember]
        ACC_RELEASE_COMMISSIONS_INLINE,

        [EnumMember]
        ACC_RELEASE_COMMISSIONS_PRORATE,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_DEPOSIT_VOUCHER,

        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_RETENTION_RECEIPT,

        [EnumMember]
        ACC_CONCEPT_SOURCE_ID,

        [EnumMember]
        ACC_TRANSACTION_NUMBER,

        [EnumMember]
        ACC_ENABLED_GENERAL_LEGDER,

        [EnumMember]
        ACC_PAYMENT_TYPE_CONSIGNMENT_CHECK,

        [EnumMember]
        ACC_RULE_PACKAGE_COLLECT,

        [EnumMember]
        ACC_APP_PREMIUM,

        [EnumMember]
        ACC_APP_PREMIUM_COMMISS,

        [EnumMember]
        ACC_RULE_PACKAGE_COMMISS,

        [EnumMember]
        ACC_BRIDGE_COLLECT,

        [EnumMember]
        ACC_BRIDGE_APPLICATION,

        [EnumMember]
        ACC_BRIDGE_PAYMENT_BALLOT,

        //[EnumMember]
        //ACC_RULE_PACKAGE_JOURNAL_ENTRY,

        [EnumMember]
        ACC_JOURNAL_ENTRY_PREMIUM,

        [EnumMember]
        ACC_CHECK_REJECTION,

        [EnumMember]
        ACC_CHECK_LEGALIZE,

        [EnumMember]
        ACC_CHECK_REGULARIZED,

        [EnumMember]
        ACC_CHECK_CHANGE,

        [EnumMember]
        ACC_JOURNAL_ENTRY_COMMISS,

        [EnumMember]
        ACC_BILL_CLIENT_DOC_NUMBER,

        [EnumMember]
        PAYM_PAYMENT_CURRENCY,

        [EnumMember]
        ACC_BRIDGE_COLLECT_NUMBER,

        [EnumMember]
        ACC_APPLICATION_AMOUNT_TOLERAN,

        [EnumMember]
        ACC_AUTOMATIC_APPLICATION_USER,

        [EnumMember]
        ACC_AT_ACCOUNTING_ID,

        [EnumMember]
        ACC_AT_ACCOUNTING_CONCEPT_ID,

        [EnumMember]
        ACC_PAYMENT_BALLOT_AID_CHECK,

        [EnumMember]
        ACC_PAYMENT_BALLOT_AID
    }
}
