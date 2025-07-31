using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyRatingZone : BaseRatingZone
    {
        [DataMember]
        public CompanyPrefix Prefix { get; set; }

        [DataMember]
        public CompanyBranch Branch { get; set; }
    }
}
