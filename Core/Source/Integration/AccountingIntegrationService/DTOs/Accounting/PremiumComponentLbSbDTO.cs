using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class PremiumComponentLbSbDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int LbId { get; set; }
        [DataMember]
        public int SbId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
