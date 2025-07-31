using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.underwritingService.Enums
{
    [DataContract]
    [Flags]
    public enum Status
    {
        Original = 1,
        Create = 2,
        Update = 3,
        Delete = 4,
        Error = 5
    }
}
