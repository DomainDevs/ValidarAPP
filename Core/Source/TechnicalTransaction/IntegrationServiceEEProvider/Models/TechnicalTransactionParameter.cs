using System.Runtime.Serialization;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Models
{
    [DataContract]
    public class TechnicalTransactionParameter
    {
        [DataMember]
        public int BranchId { get; set; }
    }
}
