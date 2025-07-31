using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum ClaimsKeys
    {
        [EnumMember]
        CLM_CLAIMS_MODULE,

        [EnumMember]
        CLM_IS_ENABLE_GENERAL_LEDGER,

        [EnumMember]
        GL_JOURNAL_ENTRY_TRANSACTION_NUMBER,

        [EnumMember]
        PAYM_PAYMENT_CURRENCY,

        [EnumMember]
        CLM_PACKAGE_CLAIM_NOTICE,

        [EnumMember]
        CLM_PACKAGE_CLAIM,

        [EnumMember]
        CLM_PACKAGE_PAYMENT_REQUEST,

        [EnumMember]
        CLM_PACKAGE_CHARGE_REQUEST,

        [EnumMember]
        CLM_NOTICE_CLOSED,

        [EnumMember]
        CLM_NOTICE_OPEN,

        [EnumMember]
        CLM_NOTICE_IN_PROGRESS,

        [EnumMember]
        CLM_ESTIMATION_INTERNAL_STATUS_CLOSED,

        [EnumMember]
        CLM_SALVAGE_PREFIX,

        [EnumMember]
        CLM_IS_UNIQUE_CLAIM_NUMBER,

        [EnumMember]
        CLM_LIABILITY_PREFIX,

        [EnumMember]
        CLM_PERSON_TYPE_PROVIDER,

        [EnumMember]
        CLM_PERSON_TYPE_INSURED,

        [EnumMember]
        CLM_PERSON_TYPE_THIRD,

        [EnumMember]
        CLM_PERSON_TYPE_HOLDER,

        [EnumMember]
        CLM_ESTIMATION_TYPE_SALARIES,

        [EnumMember]
        CLM_ESTIMATION_TYPE_STATUS_CLOSED,

        [EnumMember]
        CLM_ESTIMATION_TYPE_STATUS_REASON_CLOSED_WITH_PAYMENT,

        [EnumMember]
        CLM_RETENTION_ICA,

        [EnumMember]
        CLM_DEFAULT_PAYMENT_BRANCH
    }
}
