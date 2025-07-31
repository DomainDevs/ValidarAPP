using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.FinancialPlanServices.DTOs
{
    [DataContract]
    public class QuotaPlanDTO
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public DateTime ExpirationDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal OriginalValue { get; set; }
        [DataMember]
        public decimal ValuePaid { get; set; }
        [DataMember]
        public decimal AmountPending { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal Expenses { get; set; }
        [DataMember]
        public decimal ExpensesOther { get; set; }
        [DataMember]
        public decimal Financial { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
        [DataMember]
        public short StateQuota { get; set; }
        [DataMember]
        public short OriginalStateQuota { get; set; }
    }
}
