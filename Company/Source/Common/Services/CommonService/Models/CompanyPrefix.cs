using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    /// <summary>
    /// Ramo Comercia
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Base.BasePrefix" />
    [DataContract]
    public class CompanyPrefix : BasePrefix
    {
        [DataMember]
        public CompanyPrefixType PrefixType { get; set; }
        [DataMember]
        public List<CompanyLineBusiness> LineBusiness { get; set; }  

    }
}
