using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ModelServices.Models
{
    [DataContract]
    public class CompanyPrefix : Prefix
    {
        [DataMember]
        public CompanyAditionalInformationPrefix AditionalInformation { get; set; }

        [DataMember]
        public bool HasDetailCommiss { get; set; }

    }
}
