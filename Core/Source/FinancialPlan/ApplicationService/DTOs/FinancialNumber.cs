using System.Runtime.Serialization;

namespace Sistran.Core.Application.FinancialPlanServices.DTOs
{
    [DataContract]
    public class FinancialNumber
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Number { get; set; }
    }
}
