using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums
{

    /// <summary>
    /// Emun gneral ledger
    /// </summary>
    [DataContract]
    [Flags]
    public enum GeneralLederKeys
    {
        [EnumMember]
        GL_BUSINESS_EXCEPTION_MSJ,
        [EnumMember]
        GL_UNHANDLED_EXCEPTION_MSJ,
        [EnumMember]
        GL_JOURNAL_ENTRY_TRANSACTION_NUMBER,
        [EnumMember]
        GL_DAILY_ACCOUNTING,
        [EnumMember]
        GL_AUTOMATIC_ENTRIES,
        [EnumMember]
        GL_ENTRY_DESTINATION_BOTH,
        [EnumMember]
        GL_LEDGER_ENTRY_MODULE,
        [EnumMember]
        GL_ACCOUNTING_COMPANY_BY_DEFAULT,
        [EnumMember]
        GL_EXERCISE_CLOSING,
        [EnumMember]
        GL_INCOME_OUT_COME_CANCELLATION_ENTRY,
        [EnumMember]
        GL_PROFIT_OPENING_CLOSING_ENTRY,
        [EnumMember]
        GL_ENTRY_DESTINATION_LOCAL,
        [EnumMember]
        GL_YEARS_PROFIT_ACCOUNT_ID,
        [EnumMember]
        GL_YEARS_LOSS_ACCOUNT_ID,
        [EnumMember]
        GL_BANK_RECONCILIATION_DEPOSIT,
        [EnumMember]
        GL_DEPOSIT_BALLOT_CASH_VALUE_TYPE_ID,
        [EnumMember]
        GL_DEPOSIT_BALLOT_COMISSIONS_VALUE_TYPE_ID,
        [EnumMember]
        CLM_PERSON_TYPE_PROVIDER,
        [EnumMember]
        GL_ACCOUNTING_ACCOUNT_CASH,
        [EnumMember]
        GL_ACCOUNTING_ACCOUNT_BRIDGE,
		[EnumMember]
        ACL_CLAIMS_MODULE
    }
}
