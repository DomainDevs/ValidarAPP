using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    [DataContract]
    public class Company
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
