using System;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UnderwritingServices.Enums
{
    [Flags]
    public enum EventTypes
    {
        /// <summary>
        /// Subscription
        /// </summary>
        [EnumMember]
        Subscription = 1,
        /// <summary>
        /// Endorsement
        /// </summary>
        [EnumMember]
        Endorsement = 2,
        /// <summary>
        /// Printing
        /// </summary>
        [EnumMember]
        Printing = 3,
        /// <summary>
        /// Release
        /// </summary>
        [EnumMember]
        Release = 4,
        /// <summary>
        /// Subscription Massive
        /// </summary>
        [EnumMember]
        SubscriptionMassive = 5,
        /// <summary>
        /// Printing Massive
        /// </summary>
        [EnumMember]
        PrintingMassive = 6
    }

    [Flags]
    public enum GroupEvent
    {
        [EnumMember]
        RADICACIÓN =8
    }
}
