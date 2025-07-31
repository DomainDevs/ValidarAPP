using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class PolicyDTO: BasePolicy
    {
        [DataMember]
        public CompanySummary Summary { get; set; }

    }
}
