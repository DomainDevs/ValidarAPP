using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyThirdDeclinedType 
    {
        [DataMember]
        public decimal Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmalDescription { get; set; }

    }
}
