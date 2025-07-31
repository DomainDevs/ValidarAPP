using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyBankBranch : BaseBankBranch
    {
        [DataMember]
        public CompanyBank Bank { get; set; }
    }
}
