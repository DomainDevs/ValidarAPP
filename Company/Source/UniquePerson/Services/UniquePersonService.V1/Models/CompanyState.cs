using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyState : BaseState
    {
        [DataMember]
        public CompanyCountry Country { get; set; }
        [DataMember]
        public List<CompanyCity> Cities { get; set; }
    }
}
