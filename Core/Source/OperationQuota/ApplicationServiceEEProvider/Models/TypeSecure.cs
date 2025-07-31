using System.Runtime.Serialization;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models
{
    [DataContract]
    public class TypeSecure
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
