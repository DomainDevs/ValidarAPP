using System.Runtime.Serialization;
namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamLineBusiness
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}