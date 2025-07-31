
using System.Runtime.Serialization;

namespace Sistran.Core.Application.OperationQuotaServices.DTOs
{
    [DataContract]
    public class TypeSecureDTO
    {
        [DataMember]
        public bool IsEconomicGroup { get; set; }

        [DataMember]
        public bool IsConsortium { get; set; }

        [DataMember]
        public bool IsIndividual { get; set; }

        [DataMember]
        public bool IsNotIndividual { get; set; }
    }
}
