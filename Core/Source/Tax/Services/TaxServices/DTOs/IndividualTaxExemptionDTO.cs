using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.DTOs
{
    [DataContract]
    public class IndividualTaxExemptionDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int TaxCode { get; set; }

        [DataMember]
        public int ExemptionPercentage { get; set; }
    }
}
