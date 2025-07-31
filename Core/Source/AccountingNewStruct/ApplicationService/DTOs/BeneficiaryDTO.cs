using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class BeneficiaryDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int CustomerType { get; set; }
    }
}
