using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    /// <summary>
    /// CouponStatusTypes: Tipo del Estado del Debito 
    /// </summary>
    [DataContract]
    public enum  CouponStatusTypes
    {
        [EnumMember]
        Rejected = 1,
        [EnumMember]
        Applied = 2
    }
}
