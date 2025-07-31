using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum ExerciseTypes
    {
        [EnumMember]
        LastRenewal = 1,
        [EnumMember]
        LastMovement = 2,
        [EnumMember]
        OldEndorsement = 3,
        [EnumMember]
        PolicyDate = 4
    }
}