using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models

{
    [DataContract]
    public class CompanyEconomicActivity
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
