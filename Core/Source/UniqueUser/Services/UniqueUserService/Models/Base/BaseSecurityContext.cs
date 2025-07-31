using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseSecurityContext
    {
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Code 
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Assigned 
        /// </summary>
        [DataMember]
        public bool Assigned { get; set; } = false;

    }
}
