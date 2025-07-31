using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Framework.DAF;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{

    public class CompanyEntity  
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }

    }
}
