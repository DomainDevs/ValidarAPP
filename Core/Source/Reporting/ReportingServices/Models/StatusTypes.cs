using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models
{
    public enum StatusTypes
    {
        [EnumMember]
        Success = 1,
        [EnumMember]
        Failure = 99
    }
}
