using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    /// <summary>
    /// ExclusionTypes: Tipo de Exclusiones
    /// </summary>
    [DataContract]
    public enum ExclusionTypes
    {
        [EnumMember]
        Policy = 1,
        [EnumMember]
        Agent = 2,
        [EnumMember]
        Insured = 3
    }
}
