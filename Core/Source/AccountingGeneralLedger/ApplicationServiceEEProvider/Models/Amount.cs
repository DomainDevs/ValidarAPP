using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    [DataContract]
    public class Amount 
    {
        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public Currency Currency { get; set; }
    }
}
