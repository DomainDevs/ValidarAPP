using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Enums
{
    [DataContract]
    [Flags]
    public enum ExecutionTypes
    {
        UniqueRate = 1,
        BusinessRules = 2
    }
}
