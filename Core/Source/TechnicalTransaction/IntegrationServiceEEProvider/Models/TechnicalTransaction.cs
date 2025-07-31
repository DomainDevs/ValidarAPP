using System.Runtime.Serialization;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.EEProvider.Models
{
    [DataContract]
    public class TechnicalTransaction
    {
        [DataMember]
        public int Id { get; set; }
    }
}
