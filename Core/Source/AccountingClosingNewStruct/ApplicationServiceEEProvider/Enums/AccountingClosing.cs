using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Enums
{
    /// <summary>
    /// Modulos
    /// </summary>
    [Flags]
    public enum AccountingClosingModule
    {
        [EnumMember]
        ACL_ISSUANCE_MODULE,
        [EnumMember]
        ACL_CLAIMS_MODULE,
        [EnumMember]
        ACL_REINSURANCE_MODULE,
        [EnumMember]
        ACL_TECHNICAL_RESERVE_MODULE,
        [EnumMember]
        ACL_INCOME_AND_EXPENSES_MODULE,
        [EnumMember]
        ACL_IBNR_MODULE,
        [EnumMember]
        ACL_RISK_PREVENTION_MODULE,
        [EnumMember]
        ACL_CATASTROPHIC_RISK_RESERVE_MODULE,
        [EnumMember]
        ACL_EXPIRED_PREMIUMS_MODULE
    }
    /// <summary>
    /// TIPO DE COMPONENTE
    /// </summary>
    [Flags]
    public enum AccountingClosingComponentType
    {
        [EnumMember]
        ACL_PRIME,
        [EnumMember]
        ACL_ADMINISTRATIVE_SURCHARGES,
        [EnumMember]
        ACL_FINANCIAL_SURCHARGES,
        [EnumMember]
        ACL_ISSUANCE_RIGHTS,
        [EnumMember]
        ACL_TAXES,
        [EnumMember]
        ACL_BONUSES
    }
    /// <summary>
    /// Generales accounting closing
    /// </summary>
    [Flags]
    public enum AccountingClosing
    {
        [EnumMember]
        ACL_MODULE_DATE_ACCOUNTING,

        [EnumMember]
        ACL_AUTOMATIC_ENTRIES,

        [EnumMember]
        ACL_DESTINATION_LOCAL,
        
        [EnumMember]
        ACL_REINSURANCE_ACCOUNTING_CLOSING_PAGE_SIZE_PARAMETER,
        
        [EnumMember]
        ACL_CLOSING_RISK_RESERVE_ACCOUNTING_ACCOUNT_ID,

        [EnumMember]
        ACL_MODULE_DATE_ACCOUNTING_CLOSING,

        [EnumMember]
        ACL_PAGE_SIZE_REPORT,

        [EnumMember]
        ACL_WORD_RESERVE,

        [EnumMember]
        ACL_UTILITY_ACCOUNTING_ACCOUNT,

        [EnumMember]
        ACL_LOSS_ACCOUNTING_ACCOUNT
    }

    /// <summary>
    /// Producción de primas
    /// </summary>
    [Flags]
    public enum AccountingClosingPrimeProduction
    {
        [EnumMember]
        ACL_FORMAT_PRODUCTION_DETAIL,

        [EnumMember]
        ACL_TEMPLATE_NAME_PRODUCTION_DETAIL,

        [EnumMember]
        ACL_PROCEDURE_PRODUCTION_DETAIL,

        [EnumMember]
        ACL_PROCEDURE_GET_PRODUCTION_DETAIL
    }

    [Flags]
    public enum AccountingClosingCancellationRecord
    {
        [EnumMember]
        ACL_FORMAT_CANCELLATION_RECORD_ISSUANCE,

        [EnumMember]
        ACL_TEMPLATE_NAME_CANCELLATION_RECORD_ISSUANCE,

        [EnumMember]
        ACL_PROCEDUTE_CANCELLATION_RECORD_ISSUANCE,

        [EnumMember]
        ACL_PROCEDURE_GET_CANCELLATION_RECORD_ISSUANCE
    }
}
