using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class QuotaComponentsDTO
    {
        [DataMember]
        public int QuotaNumber { get; set; }

        [DataMember]
        public int PayerId { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

    }
}
