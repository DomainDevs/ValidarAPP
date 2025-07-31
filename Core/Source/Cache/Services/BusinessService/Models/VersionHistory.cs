using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Cache.CacheBusinessService.Models
{
    [DataContract]
    public class VersionHistory 
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Node
        /// </summary>
        [DataMember]
        public string Guid { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        [DataMember]
        public int? UserId { get; set; }

        /// <summary>
        /// VersionDatetime
        /// </summary>

        [DataMember]
        public DateTime VersionDatetime { get; set; }

    }
}
