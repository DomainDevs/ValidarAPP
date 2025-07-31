using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.DTOs
{
    [DataContract]
    public class IndividualTaxAttributeDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public int TaxAttributeId { get; set; }

        [DataMember]
        public string TaxAttributeDescription { get; set; }
    }
}
