using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.FinancialPlanServices.DTOs
{
    [DataContract]
    public class FinancialDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Tax { get; set; }

        [DataMember]
        public decimal Expenses { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public List<QuotaPlanDTO> Quotas { get; set; }
    }
}
