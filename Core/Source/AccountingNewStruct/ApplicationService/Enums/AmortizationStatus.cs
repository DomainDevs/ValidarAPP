using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{

    [DataContract]
    public enum AmortizationStatus
    {
        [EnumMember]
        Actived = 1,
        [EnumMember]
        Applied = 2, 
        [EnumMember]
        Rejected =3 ,
        [EnumMember]
        NoData = 4
    }
}
