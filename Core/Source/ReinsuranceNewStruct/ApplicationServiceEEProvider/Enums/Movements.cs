using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum Movements
    {
        Original = 1,
        Counterpart = 2,
        Adjustment = 3
    }
}
