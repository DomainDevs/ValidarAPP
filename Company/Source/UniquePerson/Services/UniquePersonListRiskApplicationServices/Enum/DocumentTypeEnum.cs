
using System;
using System.Configuration;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum
{
    [Flags]
    public enum DocumentTypeEnum
    {
        [EnumMember]
        CEDULADECIUDADANIA = 1,
        [EnumMember]
        NIT =2,
        [EnumMember]
        CEDULADEEXTRANJERIA =3,
        [EnumMember]
        TARJETADEINDENTIDAD =4,
        [EnumMember]
        NUIP = 5,
        [EnumMember]
        REGISTROCIVIL =6,
    }
}
