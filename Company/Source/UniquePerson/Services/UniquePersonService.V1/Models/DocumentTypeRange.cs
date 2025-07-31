using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class DocumentTypeRange
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public CompanyDocumentType CardTypeCode { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string CardNumberFrom { get; set; }

        [DataMember]
        public string CardNumberTo { get; set; }


    }
}
