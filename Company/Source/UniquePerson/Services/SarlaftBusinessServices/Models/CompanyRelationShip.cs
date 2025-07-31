using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{

    public class CompanyRelationShip
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
