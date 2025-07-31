using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyPhone : BasePhone
    {
        public CompanyPhoneType PhoneType { get; set; }
    }
}
