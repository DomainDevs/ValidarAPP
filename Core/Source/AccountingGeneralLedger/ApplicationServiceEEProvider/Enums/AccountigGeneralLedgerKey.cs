using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Enums
{

    [DataContract]
    [Flags]
    public enum AccountingGeneralLedgerKey
    {
        [EnumMember]
        GL_JOURNAL_ENTRY_TRANSACTION_NUMBER,
        [EnumMember]
        CLM_CLAIMS_MODULE,
        [EnumMember]
        ACC_MODULE_DATE_ACCOUNTING,
        [EnumMember]
        ACC_BANK_RECONCILIATION_RETURNED_CHECK,
        [EnumMember]
        ACC_MODULE_DATE_COLLECTING,
        [EnumMember]
        ACC_BANK_RECONCILIATION_RETURNED_CARD,
        [EnumMember]
        ACC_RULE_PACKAGE_COLLECT,
        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK,
        [EnumMember]
        ACC_PARAM_PAYMENT_METHOD_CASH,
        [EnumMember]
        ACC_CHECK_REJECTION
    }
}
