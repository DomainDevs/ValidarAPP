using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Enums
{
    [DataContract]
    [Flags]
    public enum Genders
    {
        Male = 0,
        Female = 1
    }
}
