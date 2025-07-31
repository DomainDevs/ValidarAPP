using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.Models
{
    [DataContract]
    public class CompanyAttorney: BaseAttorney
    {
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }
    }
}
