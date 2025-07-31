using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    [DataContract]
    public class Tax
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
