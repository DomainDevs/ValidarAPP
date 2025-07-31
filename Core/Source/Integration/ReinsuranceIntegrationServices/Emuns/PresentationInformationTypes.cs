using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.Enums
{
    [DataContract]
    [Flags]
    public enum PresentationInformationTypes
    {
        [EnumMember]
        Monthly = 1,
        [EnumMember]
        Quarterly = 2,
        [EnumMember]
        Biannual = 3,
        [EnumMember]
        Annual = 4,
    }
}
