using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyAddress : BaseAddress
    {
        [DataMember]
        public CompanyAddressType AddressType { get; set; }
        [DataMember]
        public City City { get; set; }
    }

    [DataContract]
    public class Prueba<T> 
    {
        [DataMember]
        public T Address { get; set; }
       
    }
}
