using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Enums
{
    [Flags]
    public enum ReinsuranceKeys
    {
        [EnumMember]
        REINS_BY_LINE_BUSINESS,

        [EnumMember]
        REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS,

        [EnumMember]
        REINS_BY_OPERATION_TYPE_PREFIX,

        [EnumMember]
        REINS_BY_INSURED,

        [EnumMember]
        REINS_BY_PREFIX,

        [EnumMember]
        REINS_BY_POLICY,

        [EnumMember]
        REINS_BY_FACULTATIVE_ISSUE,

        [EnumMember]
        REINS_BY_INSURED_PREFIX,

        [EnumMember]
        REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS_RISK,

        [EnumMember]
        REINS_BY_PREFIX_RISK,

        [EnumMember]
        REINS_BY_POLICY_LINE_BUSINESS_SUB_LINE_BUSINESS,

        [EnumMember]
        REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESSCOVERAGE,

        [EnumMember]
        REINS_BY_PREFIX_PRODUCT
    }
}
