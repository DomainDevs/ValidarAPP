using System.Runtime.Serialization;

namespace Sistran.Core.Application.FinancialPlanServices.DTOs
{
    [DataContract]
    public class ComponentBaseDTO
    {
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Tax { get; set; }

        [DataMember]
        public decimal Expenses { get; set; }

        [DataMember]
        public decimal Total { get; set; }

    }
}
