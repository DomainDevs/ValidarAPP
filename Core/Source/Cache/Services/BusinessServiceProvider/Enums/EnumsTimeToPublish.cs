using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Enums
{
    [Flags]
    public enum TimeToPublish
    {
        /// <summary>
        /// Hours
        /// </summary>
        [EnumMember]
        Hours = 0,
        
        /// <summary>
        /// Minutes
        /// </summary>
        [EnumMember]
        Minutes = 5,
        
        /// <summary>
        /// Seconds
        /// </summary>
        [EnumMember]
        Seconds = 10
    }
}
