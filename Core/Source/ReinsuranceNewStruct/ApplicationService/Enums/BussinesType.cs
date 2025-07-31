using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Enums
{
    [DataContract]
    [Flags]
    public enum  BussinesType
    {
        [EnumMember]
        [Description("100 % Compañia")]
        CompanyPercentage = 1,
        [EnumMember]
        [Description("Coaseguro aceptado")]
        Accepted = 2,
        [EnumMember]
        [Description("Coaseguro cedido")]
        Assigned = 3,
    }
}
