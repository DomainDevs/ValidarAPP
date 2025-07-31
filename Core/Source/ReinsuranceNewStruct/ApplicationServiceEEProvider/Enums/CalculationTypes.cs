using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum CalculationTypes
    {
        [EnumMember]
        Percentage = 1, 
         [EnumMember]
        Mileage = 2, 
         [EnumMember]
        Amount = 3, 
         [EnumMember]
        Millon = 4, 

    }
}
