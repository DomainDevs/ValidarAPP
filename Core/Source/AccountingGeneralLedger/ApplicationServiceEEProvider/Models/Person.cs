using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public int UserId { get; set; }
    }
}
