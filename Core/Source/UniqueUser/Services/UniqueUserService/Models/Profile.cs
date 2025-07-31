using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class Profile : BaseProfile
    {
        /// <summary>
        /// Accesses asignados
        /// </summary>
        [DataMember]
        public List<AccessProfile> profileAccesses { get; set; }

        /// <summary>
        /// contragarantias
        /// </summary>
        [DataMember]
        public List<ProfileGuaranteeStatus> guaranteeProfileStatus { get; set; }

    }
}
