
namespace Sistran.Company.Application.UniqueUserServices.Models
{
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models.Base;
    [DataContract]
    public class CompanyProfile : BaseProfile
    {
        /// <summary>
        /// Accesses asignados
        /// </summary>
        [DataMember]
        public List<AccessProfile> profileAccesses { get; set; }
    }
}
