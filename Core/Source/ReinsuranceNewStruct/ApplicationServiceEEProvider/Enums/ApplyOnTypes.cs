using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum ApplyOnTypes
    {
        [EnumMember]
        RetainedRiskSum = 1, 
         [EnumMember]
        ExcessRetention = 2,

    }
}
