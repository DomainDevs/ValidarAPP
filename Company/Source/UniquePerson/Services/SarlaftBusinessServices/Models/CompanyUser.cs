using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{

    public class CompanyUser  
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string FormNum { get; set; }
    }
}
