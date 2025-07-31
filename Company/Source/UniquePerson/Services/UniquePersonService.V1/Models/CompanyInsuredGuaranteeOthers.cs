using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class CompanyInsuredGuaranteeOthers : BaseInsuredGuarantee
    {

        [DataMember]
        public string DescriptionOthers { get; set; }
    }
}
