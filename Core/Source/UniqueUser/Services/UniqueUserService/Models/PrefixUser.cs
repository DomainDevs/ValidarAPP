using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class PrefixUser
    {
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// PrefixCode
        /// </summary>
        [DataMember]
        public int PrefixCode { get; set; }

    }
}
