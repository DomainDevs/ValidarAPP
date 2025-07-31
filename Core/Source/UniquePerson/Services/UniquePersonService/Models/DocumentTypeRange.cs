using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UniquePersonServiceIndividual.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    [DataContract]
    public class DocumentTypeRange
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DocumentType CardTypeCode { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string CardNumberFrom { get; set; }

        [DataMember]
        public string CardNumberTo { get; set; }

        [DataMember]
        public int? IndividualTypeId { get; set; }

        [DataMember]
        public string DescriptionIndividualType { get; set; }

    }
}
