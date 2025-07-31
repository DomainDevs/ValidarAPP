using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Enums
{
    public enum PersonKeys
    {
        [EnumMember]
        PER_ROL_REINSURER,

        [EnumMember]
        PER_ROL_AGENT,

        [EnumMember]
        PER_ROL_INSURED,

        [EnumMember]
        PER_ROL_SUPPLIER,

        [EnumMember]
        PER_ROL_CONINSURED,

        [EnumMember]
        PER_ROL_THIRD,

        [EnumMember]
        PER_ROL_EMPLOYEE
    }
}
