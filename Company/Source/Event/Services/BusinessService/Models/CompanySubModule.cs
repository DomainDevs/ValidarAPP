
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.BusinessService.Models
{
    [DataContract]
    public class CompanySubModule : GenericList
    {
        [DataMember]
        public int ModuleId { get; set; }
    }
}