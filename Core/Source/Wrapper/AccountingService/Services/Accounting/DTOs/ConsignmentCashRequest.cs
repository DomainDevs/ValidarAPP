using System.Runtime.Serialization;

namespace Sistran.Core.Application.WrapperAccountingService.DTOs
{
    /// <summary>
    /// Consignacion Bancaria
    /// </summary>
    [DataContract]
    public class ConsignmentCashRequest
    {
        [DataMember]
        public int BankId { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string DepositorName { get; set; }
        [DataMember]
        public int Currency { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string BallotNumber { get; set; }

    }
}
