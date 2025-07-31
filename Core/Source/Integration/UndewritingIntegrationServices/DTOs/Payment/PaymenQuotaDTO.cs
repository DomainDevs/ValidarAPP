using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Payment
{
    [DataContract]
    public class PaymenQuotaDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public short StateQuota { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }

    }
}
