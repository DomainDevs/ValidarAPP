using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class PaymentReinsuranceDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PaymentNumber { get; set; }
        [DataMember]
        public DateTime PaymentDate { get; set; }
        [DataMember]
        public int Movements { get; set; }
        [DataMember]
        public MovementTypeDTO MovementType { get; set; }
        [DataMember]
        public int RiskNumber { get; set; }
        [DataMember]
        public int CoverageNumber { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public AmountDTO Amount { get; set; }
        [DataMember]
        public int ClaimId { get; set; }
    }
}