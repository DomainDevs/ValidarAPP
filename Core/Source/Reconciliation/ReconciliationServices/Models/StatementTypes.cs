
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReconciliationServices.Models
{
    /// <summary>
    /// StatementTypes: Tipo de Extracto
    /// </summary>
    [DataContract]
     public enum  StatementTypes
    {
        [EnumMember]
        Bank = 1,
        [EnumMember]
        CentralAccounting=2,
        [EnumMember]
        DailyAccounting=3,
     }
}
