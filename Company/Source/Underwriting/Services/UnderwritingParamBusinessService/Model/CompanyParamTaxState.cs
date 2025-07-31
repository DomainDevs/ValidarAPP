using System.Runtime.Serialization;
namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamTaxState
    {
        [DataMember]
        public int? IdState { get; set; }

        [DataMember]
        public int? IdCity { get; set; }

        [DataMember]
        public int? IdCountry { get; set; }

        [DataMember]
        public string DescriptionState { get; set; }
        [DataMember]
        public string DescriptionCity { get; set; }

        [DataMember]
        public string DescriptionCountry { get; set; }
    }
}